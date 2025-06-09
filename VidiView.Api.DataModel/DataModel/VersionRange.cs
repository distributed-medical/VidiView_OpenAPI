using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.DataModel;

public record VersionRange
{
    /// <summary>
    /// The intended minimum version
    /// </summary>
    public string? Min { get; init; }

    /// <summary>
    /// The intended maximum version
    /// </summary>
    /// <remarks>This is normally the version of the tested client application</remarks>
    public string? Max { get; init; }
}
