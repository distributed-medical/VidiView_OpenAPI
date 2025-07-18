using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Exceptions;
public class E1505_SettingNotAvailableException : VidiViewException
{
    public E1505_SettingNotAvailableException(string message)
        : base(message)
    {
        ErrorCode = 1505;
    }
}
