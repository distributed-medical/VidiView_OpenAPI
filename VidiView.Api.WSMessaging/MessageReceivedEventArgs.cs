using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.WSMessaging;
public class MessageReceivedEventArgs : EventArgs
{
    internal MessageReceivedEventArgs(WSMessage message)
    {
        Message = message;
    }

    public WSMessage Message { get; }
}
