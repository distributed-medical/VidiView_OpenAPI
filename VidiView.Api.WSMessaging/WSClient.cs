using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace VidiView.Api.WSMessaging;

public class WSClient
{
    const string SubProtocol = "vidiview.com+json";

    readonly byte[] _receiveBuffer = new byte[16 * 1024];
    readonly SemaphoreSlim _receiveLock = new SemaphoreSlim(1);
    readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1);
    readonly ILogger _logger;
    readonly ConcurrentDictionary<string, TaskCompletionSource<ResponseMessage>> _tasks = new();

    ClientWebSocket? _socket;

    /// <summary>
    /// Event raised when new message arrives
    /// </summary>
    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    public WSClient(ILogger<WSClient>? logger = null)
    {
        _logger = logger ?? NullLogger<WSClient>.Instance;
    }

    /// <summary>
    /// Max message size 
    /// </summary>
    public int MaxMessageSize { get; set; } = 2 * 1024 * 1024;

    public TimeSpan SendMessageTimeout { get; set; } = TimeSpan.FromSeconds(20);

    /// <summary>
    /// Returns true when connected to remote host
    /// </summary>
    public bool IsConnected => _socket != null;


    /// <summary>
    /// Connect to VidiView WebSocket and authenticate connection
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="apiKey"></param>
    /// <param name="authenticationToken"></param>
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
            var authMessage = new AuthenticateMessage(apiKey, authorization);

            await SendMessageInternalAsync(authMessage, socket, cancellationToken);
            var message = await ReadMessageInternalAsync(socket, cancellationToken);

            if (message is ResponseMessage response && response.InResponseTo == authMessage.MessageId)
            {
                // Successfully connected
                _socket = socket;
                ReadMessageLoop(socket);
            }
            else
            {
                throw new Exception("Unexpected response to authenticate request");
            }
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Close socket connection
    /// </summary>
    /// <returns></returns>
    public async Task CloseAsync()
    {
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

                // Close nicely
                await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);

                try
                {
                    // Wait for close operation to complete
                    var message = await ReadMessageInternalAsync(_socket,
                        new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);

                    if (message == null)
                    {
                        // Another read operation is in progress, which will
                        // receive the Close response. This is OK
                        await Task.Delay(100); // Ensure message is processed
                    }
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
    /// Post message without waiting for any response
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async void PostMessage(WSMessage message)
    {
        await SendMessageInternalAsync(message, null, CancellationToken.None);
    }

    /// <summary>
    /// Send a message and wait for response. Default timeout <see cref="SendMessageTimeout"/> is applied
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task<ResponseMessage> SendMessageAsync(WSMessage message)
    {
        try
        {
            return await SendMessageAsync(message,
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
    public async Task<ResponseMessage> SendMessageAsync(WSMessage message, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<ResponseMessage>();
        var success = _tasks.TryAdd(message.MessageId, tcs);
        if (!success)
            throw new InvalidOperationException("This message is already being awaited");

        await SendMessageInternalAsync(message, null, cancellationToken);
        return await tcs.Task;
    }

    /// <summary>
    /// Serialize message as Json and send to host
    /// </summary>
    /// <param name="message"></param>
    /// <param name="socket">Specific socket to use. Set to null to use connected socket</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task SendMessageInternalAsync(WSMessage message, ClientWebSocket? socket, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        var data = MessageSerializer.Serialize(message);
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
    private async void ReadMessageLoop(ClientWebSocket socket)
    {
        _logger.LogInformation("Read message loop started");
        try
        {
            while (true)
            {
                var message = await ReadMessageInternalAsync(socket, CancellationToken.None);
                if (message is ResponseMessage response)
                {
                    // Mark corresponding call as completed
                    if (_tasks.TryRemove(response.InResponseTo, out var tcs))
                    {
                        tcs.SetResult(response);

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
            _logger.LogError(ex, "Error reading message");
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
    async Task<WSMessage?> ReadMessageInternalAsync(ClientWebSocket socket, CancellationToken cancellationToken)
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
                var result = await socket.ReceiveAsync(_receiveBuffer, cancellationToken).ConfigureAwait(false);
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:

                        // Received message from remote client
                        if (result.EndOfMessage && concatenatedStream == null)
                        {
                            _logger.LogDebug("Full message received ({bytes} bytes)", result.Count);
                            var success = MessageSerializer.TryDeserialize(new ArraySegment<byte>(_receiveBuffer, 0, result.Count), out var message);
                            if (!success)
                                _logger.LogWarning("Failed to deserialize message");

                            return message;
                        }
                        else
                        {
                            _logger.LogTrace("Partial message received ({bytes} bytes)", result.Count);

                            // The entire message didn't fit in our local buffer
                            // Copy to temporary stream instead
                            if (concatenatedStream == null)
                            {
                                concatenatedStream = new MemoryStream(_receiveBuffer.Length * 8);
                            }

                            concatenatedStream.Write(_receiveBuffer, 0, result.Count);
                            if (concatenatedStream.Length > MaxMessageSize)
                            {
                                _logger.LogError("Input message too large (> {bytes}). Closing connection", MaxMessageSize);

                                await socket.CloseAsync(WebSocketCloseStatus.MessageTooBig, "Input message too large", CancellationToken.None);
                                throw new Exception($"Input message larger than {MaxMessageSize} bytes");
                            }

                            if (!result.EndOfMessage)
                                continue; // Read more data

                            concatenatedStream.Flush();

                            _logger.LogDebug("Full message assembled ({bytes} bytes)", concatenatedStream.Length);
                            var success = MessageSerializer.TryDeserialize(
                                new ArraySegment<byte>(concatenatedStream.GetBuffer(), 0, (int)concatenatedStream.Length),
                                out var message);
                            if (!success)
                                _logger.LogWarning("Failed to deserialize message");

                            return message;
                        }

                    case WebSocketMessageType.Close:
                        _logger.LogInformation("Connection closed. {status}: {reason}", result.CloseStatus, result.CloseStatusDescription);
                        _socket = null; // IsConnected => False
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
        finally
        {
            _receiveLock.Release();
        }
    }
}
