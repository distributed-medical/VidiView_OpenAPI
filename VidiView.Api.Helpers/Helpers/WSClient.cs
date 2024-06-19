using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using VidiView.Api.DataModel;
using VidiView.Api.Exceptions;
using VidiView.Api.WSMessaging;

namespace VidiView.Api.Helpers;

public class WSClient
{
    public const string DefaultSubProtocol = "vidiview.com+json";

    readonly byte[] _receiveBuffer = new byte[16 * 1024];
    readonly SemaphoreSlim _receiveLock = new SemaphoreSlim(1);
    readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1);
    readonly ILogger _logger;
    readonly ConcurrentDictionary<string, TaskCompletionSource<IWSReply>> _messageAwaitingReply = new();

    ClientWebSocket? _socket;
    CancellationTokenSource? _cts;

    /// <summary>
    /// Event raised when new message arrives
    /// </summary>
    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    /// <summary>
    /// Raised when the connection is closed
    /// </summary>
    public event EventHandler<ConnectionClosedEventArgs>? ConnectionClosed;

    public WSClient(ILogger<WSClient>? logger = null)
    {
        _logger = logger ?? NullLogger<WSClient>.Instance;
    }

    /// <summary>
    /// The sub-protocol to use
    /// </summary>
    public string SubProtocol { get; init; } = DefaultSubProtocol;

    /// <summary>
    /// Max message size 
    /// </summary>
    public int MaxMessageSize { get; set; } = 2 * 1024 * 1024;

    /// <summary>
    /// Timeout for sending messages
    /// </summary>
    public TimeSpan SendMessageTimeout { get; set; } = TimeSpan.FromSeconds(20);

    /// <summary>
    /// Returns true when connected to remote host
    /// </summary>
    public bool IsConnected => _socket != null;

    /// <summary>
    /// Returns the connected user
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Connect to VidiView Web Socket and authenticate connection
    /// </summary>
    /// <param name="uri">The Uri to call</param>
    /// <param name="apiKey">The application API key. This is the value of the ApiKey header to call the API</param>
    /// <param name="authorization">Authorization token. This is the value of the authorization bearer token to call the API</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ConnectAsync(Uri uri, string apiKey, string authorization, CancellationToken cancellationToken)
    {
        // Create a new web socket
        _logger.LogInformation("Connect web socket to {uri}", uri);

        var socket = new ClientWebSocket();
        socket.Options.AddSubProtocol(SubProtocol);
        socket.Options.UseDefaultCredentials = false;

        try
        {
            // Negotiate web socket
            await socket.ConnectAsync(uri, cancellationToken);
            _logger.LogDebug("Web socket connected");

            // Now we are connected. The server requires us to
            // send an authentication message first of all
            var authMessage = WSMessage.Factory<AuthenticateMessage>();
            authMessage.ApiKey = apiKey;
            authMessage.Authorization = authorization;

            await SendMessageInternalAsync(authMessage, socket, cancellationToken);
            var message = await ReadMessageInternalAsync(socket, cancellationToken);

            if (message is AuthenticateReplyMessage response 
                && response.InReplyTo == authMessage.MessageId)
            {
                // Successfully connected
                User = response.User;

                _socket = socket;
                ReadMessageLoop();
            }
            else
            {
                throw new Exception("Unexpected response to authenticate request");
            }
        }
        catch
        {
            User = null;
            socket.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Close connection
    /// </summary>
    /// <returns></returns>
    public async Task CloseAsync()
    {
        // Cancel the message loop
        _cts?.Cancel();
        await Task.Delay(150);
        User = null;

        switch (_socket?.State)
        {
            case null:
            case WebSocketState.Closed:
                // Do nothing;
                break;

            case WebSocketState.CloseSent:
                _socket.Abort();
                break;

            default:
                _logger.LogInformation("Close socket requested");

                // Initiate Close Handshake
                await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure,
                    "Close requested",
                    new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

                try
                {
                    // The ReadMessageLoop will signal connection closed event
                    _logger.LogDebug("Connection successfully closed");
                }
                catch (ConnectionClosedException)
                {
                    // Expected
                    _logger.LogDebug("Connection successfully closed");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Unexpected response on Close()");
                }
                break;
        }
    }

    /// <summary>
    /// Post message without waiting for reply
    /// </summary>
    /// <param name="message"></param>
    public async Task SendAsync(IWSMessage message)
    {
        await SendMessageInternalAsync(message, null, CancellationToken.None);
    }

    /// <summary>
    /// Send a message and wait for reply. Default timeout <see cref="SendMessageTimeout"/> is applied
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException">Thrown if timeout</exception>
    /// <exception cref="ConnectionClosedException">Thrown if connection is closed</exception>
    public async Task<IWSReply> SendAndAwaitReplyAsync(IWSMessage message)
    {
        try
        {
            return await SendAndAwaitReplyAsync(message,
                new CancellationTokenSource(SendMessageTimeout).Token);
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException();
        }
    }

    /// <summary>
    /// Send a message and wait for response
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ConnectionClosedException">Thrown if connection is closed</exception>
    public async Task<IWSReply> SendAndAwaitReplyAsync(IWSMessage message, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<IWSReply>();
        var success = _messageAwaitingReply.TryAdd(message.MessageId, tcs);
        if (!success)
            throw new InvalidOperationException("This message is already being awaited");

        try
        {
            await SendMessageInternalAsync(message, null, cancellationToken);

            // Now wait for reply to be received
            return await tcs.Task.WaitAsync(cancellationToken);
        }
        finally
        {
            // Ensure we don't leave this lingering
            _messageAwaitingReply.TryRemove(message.MessageId, out _);
        }
    }

    /// <summary>
    /// Serialize message as Json and send to host
    /// </summary>
    /// <param name="message"></param>
    /// <param name="socket">Specific socket to use. Set to null to use connected socket</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task SendMessageInternalAsync(IWSMessage message, ClientWebSocket? socket, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        var data = message.Serialize();
        if (data.Length > MaxMessageSize)
        {
            _logger.LogError("Message size too large. Message size is {bytes}, max size is {bytes}", data.Length, MaxMessageSize);
            throw new OutOfMemoryException("Message too large");
        }

        await _sendLock.WaitAsync(cancellationToken);
        try
        {
            _logger.LogDebug("Send {type} {messageId} ({bytes} bytes)", message.MessageType, message.MessageId, data.Length);
            socket ??= _socket ?? throw new InvalidOperationException("Not connected");

            await socket.SendAsync(data,
                WebSocketMessageType.Text,
                true,
                cancellationToken);
        }
        catch (WebSocketException ex)
        {
            switch (ex.WebSocketErrorCode)
            {
                case WebSocketError.Faulted:
                case WebSocketError.ConnectionClosedPrematurely:
                case WebSocketError.InvalidState:
                    throw new ConnectionClosedException(WebSocketCloseStatus.EndpointUnavailable, null, ex);
            }

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Send message failed");
            throw;
        }
        finally
        {
            _sendLock.Release();
        }
    }

    /// <summary>
    /// This will read messages and fire events
    /// </summary>
    /// <param name="socket"></param>
    private async void ReadMessageLoop()
    {
        _cts = new CancellationTokenSource();
        var cancellationToken = _cts.Token;

        _logger.LogInformation("Read message loop started");
        var socket = _socket ?? throw new InvalidOperationException("Not connected");

        try
        {
            while (true)
            {
                var message = await ReadMessageInternalAsync(socket, cancellationToken);
                if (message is IWSReply reply)
                {
                    // Check if any message is waiting for reply
                    if (_messageAwaitingReply.TryRemove(reply.InReplyTo, out var tcs))
                    {
                        tcs.SetResult(reply);

                        // Response has been returned. Don't raise event
                        continue;
                    }
                }

                if (message != null)
                {
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
                }
            }
        }
        catch (Exception ex)
        {
            // Since we are here, we are no longer connected
            _socket = null;

            if (cancellationToken.IsCancellationRequested)
            {
                // This is a deliberate Close called from our side
                _logger.LogDebug(ex, "Connection closed");
                ConnectionClosed?.Invoke(this, new ConnectionClosedEventArgs(null));
            }
            else
            {
                _logger.LogWarning(ex, "Connection unexpectedly closed");
                ConnectionClosed?.Invoke(this, new ConnectionClosedEventArgs(ex));
            }

            // No use throwing here since this is declared async void
        }
        finally
        {
            _logger.LogInformation("Read message loop exited");
        }
    }

    /// <summary>
    /// Read next message from the socket. This call will block until a message is received
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    async Task<IWSMessage?> ReadMessageInternalAsync(ClientWebSocket socket, CancellationToken cancellationToken)
    {
        if (!_receiveLock.Wait(0))
        {
            _logger.LogWarning("Another receive operation is already outstanding for this socket");
            return null;
        }

        try
        {
            MemoryStream? concatenatedStream = null;

            do
            {
                // Cancelling this call will abort the socket
                var result = await socket.ReceiveAsync(_receiveBuffer, CancellationToken.None).ConfigureAwait(false);
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:

                        // Received message from remote client
                        if (result.EndOfMessage && concatenatedStream == null)
                        {
                            _logger.LogDebug("Full message received ({bytes} bytes)", result.Count);
                            var success = WSMessage.TryDeserialize(new ArraySegment<byte>(_receiveBuffer, 0, result.Count), out var message);
                            if (!success)
                                _logger.LogWarning("Failed to deserialize message");

                            return message;
                        }
                        else
                        {
                            _logger.LogTrace("Partial message received ({bytes} bytes)", result.Count);

                            // The entire message didn't fit in our local buffer
                            // Copy to temporary stream instead
                            concatenatedStream ??= new MemoryStream(_receiveBuffer.Length * 8);
                            concatenatedStream.Write(_receiveBuffer, 0, result.Count);
                            if (concatenatedStream.Length > MaxMessageSize)
                            {
                                _logger.LogError("Input message too large (> {bytes}). Closing connection", MaxMessageSize);

                                await socket.CloseAsync(WebSocketCloseStatus.MessageTooBig, "Input message too large", CancellationToken.None);
                                throw new ConnectionClosedException(WebSocketCloseStatus.MessageTooBig, $"Input message larger than {MaxMessageSize} bytes");
                            }

                            if (!result.EndOfMessage)
                                continue; // Read more data

                            concatenatedStream.Flush();

                            _logger.LogDebug("Full message assembled ({bytes} bytes)", concatenatedStream.Length);
                            var success = WSMessage.TryDeserialize(
                                new ArraySegment<byte>(concatenatedStream.GetBuffer(), 0, (int)concatenatedStream.Length),
                                out var message);
                            if (!success)
                                _logger.LogWarning("Failed to deserialize message");

                            return message;
                        }

                    case WebSocketMessageType.Close:
                        _logger.LogInformation("Connection closed. {status}: {reason}", result.CloseStatus, result.CloseStatusDescription);
                        throw new ConnectionClosedException(result.CloseStatus, result.CloseStatusDescription);

                    case WebSocketMessageType.Binary:
                        _logger.LogDebug("Binary frame ignored");
                        continue;

                    default:
                        _logger.LogDebug("Unsupported message type {type}", result.MessageType);
                        continue;
                }

                throw new Exception("Unexpected");
            } while (true);
        }
        catch (WebSocketException ex)
        {
            switch (ex.WebSocketErrorCode)
            {
                case WebSocketError.Faulted:
                case WebSocketError.ConnectionClosedPrematurely:
                case WebSocketError.InvalidState:
                    throw new ConnectionClosedException(WebSocketCloseStatus.EndpointUnavailable, null, ex);
            }

            throw;
        }
        finally
        {
            _receiveLock.Release();
        }
    }
}
