﻿namespace VidiView.Api.DataModel;

public record Study
{
    /// <summary>
    /// The unique study id
    /// </summary>
    public Guid StudyId { get; init; }

    /// <summary>
    /// The global study instance UID
    /// </summary>
    public string StudyInstanceUid { get; init; } = null!;

    /// <summary>
    /// The local time this study was created
    /// </summary>
    public DateTimeOffset StudyDate { get; init; }

    /// <summary>
    /// Study accession number
    /// </summary>
    public string? AccessionNumber { get; init; }

    /// <summary>
    /// The department info
    /// </summary>
    /// <remarks>Only used by remote parties</remarks>
    public IdAndName Department { get; init; }

    /// <summary>
    /// Study external ID 
    /// </summary>
    public string? ExternalIdentification { get; init; }

    /// <summary>
    /// The Controller location
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// The patient this study is associated with. If null, the study is unidentified
    /// </summary>
    public Patient? Patient { get; init; }

    /// <summary>
    /// The number of images present in this study
    /// </summary>
    public int? ImageCount { get; init; }

    /// <summary>
    /// Study description
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The Controller that has started this study
    /// </summary>
    public Guid? ControllerId { get; init; }

    /// <summary>
    /// Performing physician's name
    /// </summary>
    public PersonName? PerformingPhysicianName { get; init; }

    /// <summary>
    /// Performing nurse
    /// </summary>
    public PersonName? PerformingNurse { get; init; }

    /// <summary>
    /// Performing assistant
    /// </summary>
    public PersonName? PerformingAssistant1 { get; init; }

    /// <summary>
    /// Performing assistant
    /// </summary>
    public PersonName? PerformingAssistant2 { get; init; }

    /// <summary>
    /// Referring physicians name
    /// </summary>
    public PersonName? ReferringPhysicianName { get; init; }

    /// <summary>
    /// Procedure codes
    /// </summary>
    /// <remarks>Stored as XML for backward compatibility for the time being</remarks>
    public CodedValue[]? ProcedureCodes { get; init; }

    /// <summary>
    /// Any specific equipment used during this study
    /// </summary>
    public Equipment[]? Equipment { get; init; }

    /// <summary>
    /// If this is set, the study is deleted
    /// </summary>
    public DateTimeOffset? DeletedDate { get; init; }

    /// <summary>
    /// This represents the permission set granted by the Server to this Study
    /// </summary>
    public long? EffectivePermissionSet { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; init; }
}