using System.Net.WebSockets;

namespace VidiView.Api.Exceptions;
public class E1430_WebSocketClosedException : VidiViewException
{
    public E1430_WebSocketClosedException(WebSocketCloseStatus? status, string? description)
        : base($"Connection closed. {description}")
    {
        Status = status;
        Description = description;
    }
    public E1430_WebSocketClosedException(WebSocketCloseStatus? status, string? description, Exception innerException)
        : base($"Connection closed. {description}", innerException)
    {
        Status = status;
        Description = description;
    }

    public WebSocketCloseStatus? Status { get; init; }

    public string? Description { get; init; }
}
