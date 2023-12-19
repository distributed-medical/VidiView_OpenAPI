using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.DataModel;

public record StudyIdentifyOptions
{
    /// <summary>
    /// Identify study using a specific SOP instance
    /// </summary>
    public string? SopInstance { get; init; }

    /// <summary>
    /// Identify a study for a specific patient
    /// </summary>
    public Guid? PatientIdGuid { get; init; }
}
