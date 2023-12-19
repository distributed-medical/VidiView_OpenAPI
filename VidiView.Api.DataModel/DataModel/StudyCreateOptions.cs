namespace VidiView.Api.DataModel;

public record StudyCreateOptions : Study
{
    /// <summary>
    /// The SOP Instance this study is to be based on
    /// </summary>
    public string SopInstance { get; init; }

    /// <summary>
    /// Specify the patient to create this study for
    /// </summary>
    public Guid? PatientIdGuid { get; init; }
}