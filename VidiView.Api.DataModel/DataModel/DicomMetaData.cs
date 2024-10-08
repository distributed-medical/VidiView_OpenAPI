﻿namespace VidiView.Api.DataModel;

public record DicomMetaData
{
    /// <summary>
    /// Base64 encoded raw dicom metadata
    /// </summary>
    public string Base64 { get; init; }
}
