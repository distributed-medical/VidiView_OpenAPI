using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Helpers;
public enum ApiCompatibility
{
    /// <summary>
    /// Invalid response, should be treated as a major issue
    /// </summary>
    InvalidResponse,

    /// <summary>
    /// The server indicates an Api version that is too new for this client library.
    /// This should be treated as a major issue!
    /// </summary>
    ClientApiOlderThanSupported,

    /// <summary>
    /// The server indicates an Api version that is too old for this client library.
    /// This should be treated as a major issue!
    /// </summary>
    ClientApiNewerThanSupported,

    /// <summary>
    /// The server indicates an Api version that is much more recent than this
    /// client library is intended for, but it should be compatible.
    /// </summary>
    /// <remarks>This is returned when there is a major difference in versions</remarks>
    ClientApiOldButSupported,

    /// <summary>
    /// The server indicates an Api version that is old, but it is supported
    /// </summary>
    /// <remarks>This is returned when the connected server has an Api version that is 
    /// older than this library has been tested against</remarks>
    ClientApiNewButSupported,

    /// <summary>
    /// Returned when the client and server are both up to date
    /// </summary>
    UpToDate,
}
