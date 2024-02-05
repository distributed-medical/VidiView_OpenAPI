namespace VidiView.Api.DataModel;

public record StudyCreateOptions : Study
{
    /// <summary>
    /// Create unidentified study
    /// </summary>
    /// <returns></returns>
    public static StudyCreateOptions Unidentified()
    {
        return new StudyCreateOptions();
    }

    /// <summary>
    /// Create a study based on the specified patient
    /// </summary>
    /// <param name="patient"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static StudyCreateOptions ForPatient(Patient patient)
    {
        ArgumentNullException.ThrowIfNull(patient, nameof(patient));
        if (patient.SopInstance != null)
        {
            return new StudyCreateOptions { SopInstance = patient.SopInstance };
        }
        else if (patient.Id?.Guid != null && patient.Id.Guid != Guid.Empty)
        {
            return new StudyCreateOptions { PatientIdGuid = patient.Id.Guid };
        }
        else
        {
            throw new ArgumentException("The supplied patient does not have a valid Id.Guid");
        }
    }

    /// <summary>
    /// Create a study based on a scheduled study
    /// </summary>
    /// <param name="scheduledStudy"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static StudyCreateOptions ForScheduledStudy(ScheduledStudy scheduledStudy)
    {
        ArgumentNullException.ThrowIfNull(scheduledStudy, nameof(scheduledStudy));

        if (scheduledStudy.SopInstance != null)
        {
            return new StudyCreateOptions { SopInstance = scheduledStudy.SopInstance };
        }

        throw new ArgumentException("The scheduled study does not have a valid SOP instance");
    }

    /// <summary>
    /// The SOP Instance this study is to be based on
    /// </summary>
    public string? SopInstance { get; init; }

    /// <summary>
    /// Specify the patient to create this study for
    /// </summary>
    public Guid? PatientIdGuid { get; init; }
}