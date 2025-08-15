using System.Net.WebSockets;

namespace VidiView.Api.Exceptions;
public class E1431_AuthenticateWebSocketException : VidiViewException
{
    public E1431_AuthenticateWebSocketException(string? message, Exception? innerException = null)
        : base($"Failed to authenticate web socket connection. {message}", innerException)
    {
    }
}
