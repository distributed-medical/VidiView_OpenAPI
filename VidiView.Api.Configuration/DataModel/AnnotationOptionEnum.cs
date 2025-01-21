using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.Configuration.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<AnnotationOption>))]
public enum AnnotationOption
{
    Unknown,

    /// <summary>
    /// Skip annotations when exporting
    /// </summary>
    Skip,

    /// <summary>
    /// Create a Structured report for the annotaions
    /// </summary>
    SR,

    /// <summary>
    /// Burn annotation in image
    /// </summary>
    Burn
}
