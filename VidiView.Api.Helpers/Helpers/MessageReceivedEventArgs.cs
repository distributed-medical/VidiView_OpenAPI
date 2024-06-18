using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidiView.Api.WSMessaging;

namespace VidiView.Api.Helpers;
public class MessageReceivedEventArgs : EventArgs
{
    internal MessageReceivedEventArgs(WSMessage message)
    {
        Message = message;
    }

    public WSMessage Message { get; }
}
