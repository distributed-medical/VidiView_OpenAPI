using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Exceptions;
public class E1510_DecryptDataException : VidiViewException
{
    public E1510_DecryptDataException(string message)
        : base(message)
    {
        ErrorCode = 1510;
    }
    public E1510_DecryptDataException(string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = 1510;
    }
}
