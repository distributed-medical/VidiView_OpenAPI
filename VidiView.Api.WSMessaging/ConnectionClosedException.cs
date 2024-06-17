using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.WSMessaging;
public class ConnectionClosedException : Exception
{
    public ConnectionClosedException(WebSocketCloseStatus? status, string? description)
        : base($"Connection closed. {description}")
    {
        Status = status;
        Description = description;
    }

    public WebSocketCloseStatus? Status { get; }
    public string? Description { get; }
}
