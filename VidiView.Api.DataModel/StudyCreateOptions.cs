namespace VidiView.Api.DataModel;

public class StudyCreateOptions
{
    /// <summary>
    /// The SOP Instance this study is to be based on
    /// </summary>
    public string SopInstance { get; set; }

    /// <summary>
    /// The local time this study was created
    /// </summary>
    public DateTimeOffset StartTime
    {
        get; set;
    }

    /// <summary>
    /// Specify the patient to create this study for
    /// </summary>
    public Guid? PatientIdGuid
    {
        get; set;
    }

    /// <summary>
    /// Alternate way of specifying patient
    /// </summary>
    public Patient Patient
    {
        get; set;
    }

    /// <summary>
    /// Start a study based on a specific scheduled study
    /// </summary>
    public string StudyInstanceUid
    {
        get; set;
    }

    /// <summary>
    /// A specific study id guid to assign to this study
    /// </summary>
    public Guid? StudyId
    {
        get; set;
    }
}
