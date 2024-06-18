using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Helpers;
public class ConnectionClosedEventArgs : EventArgs
{
    internal ConnectionClosedEventArgs(Exception? ex)
    {
        Exception = ex;
    }

    public Exception? Exception { get; }
}
