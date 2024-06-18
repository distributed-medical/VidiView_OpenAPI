using System.Net.WebSockets;

namespace VidiView.Api.Exceptions;
public class ConnectionClosedException : Exception
{
    public ConnectionClosedException(WebSocketCloseStatus? status, string? description)
        : base($"Connection closed. {description}")
    {
        Status = status;
        Description = description;
    }
    public ConnectionClosedException(WebSocketCloseStatus? status, string? description, Exception innerException)
        : base($"Connection closed. {description}", innerException)
    {
        Status = status;
        Description = description;
    }

    public WebSocketCloseStatus? Status { get; }
    public string? Description { get; }
}
