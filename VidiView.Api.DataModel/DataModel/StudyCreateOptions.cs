namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record StudyCreateOptions : Study
{
    #region Factory methods
    /// <summary>
    /// Create unidentified study
    /// </summary>
    /// <returns></returns>
    [Obsolete("Use Unidentified(IdAndName department) instead to specify the department for the study")]
    public static StudyCreateOptions Unidentified()
    {
        return Unidentified(null!);
    }

    /// <summary>
    /// Create unidentified study
    /// </summary>
    /// <param name="department">The department in which this study is created</param>
    /// <returns></returns>
    public static StudyCreateOptions Unidentified(IdAndName department)
    {
        return new StudyCreateOptions()
        {
            Department = department,
            StudyId = Guid.NewGuid(),
            StudyDate = DateTimeOffset.Now,
        };
    }

    /// <summary>
    /// Create a study based on the specified patient
    /// </summary>
    /// <param name="patient"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    [Obsolete("Use ForPatient(IdAndName department, Patient patient) instead to specify the department for the study", false)]
    public static StudyCreateOptions ForPatient(Patient patient)
    {
        return ForPatient(null!, patient);
    }

    /// <summary>
    /// Create a study based on the specified patient
    /// </summary>
    /// <param name="department">The department in which this study is created</param>
    /// <param name="patient"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static StudyCreateOptions ForPatient(IdAndName department, Patient patient)
    {
        ArgumentNullException.ThrowIfNull(patient, nameof(patient));
        if (patient.SopInstance != null)
        {
            return new StudyCreateOptions { Department = department, SopInstance = patient.SopInstance };
        }
        else if (patient.Id?.Guid != null && patient.Id.Guid != Guid.Empty)
        {
            return new StudyCreateOptions { Department = department, PatientIdGuid = patient.Id.Guid };
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
    [Obsolete("Use ForPatient(IdAndName department, Patient patient) instead to specify the department for the study", false)]
    public static StudyCreateOptions ForScheduledStudy(ScheduledStudy scheduledStudy)
    {
        return ForScheduledStudy(null!, scheduledStudy);
    }

    /// <summary>
    /// Create a study based on a scheduled study
    /// </summary>
    /// <param name="department">The department in which this study is created</param>
    /// <param name="scheduledStudy"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static StudyCreateOptions ForScheduledStudy(IdAndName department, ScheduledStudy scheduledStudy)
    {
        ArgumentNullException.ThrowIfNull(scheduledStudy, nameof(scheduledStudy));

        if (scheduledStudy.SopInstance != null)
        {
            return new StudyCreateOptions { Department = department, SopInstance = scheduledStudy.SopInstance };
        }

        throw new ArgumentException("The scheduled study does not have a valid SOP instance");
    }
    #endregion

    /// <summary>
    /// The SOP Instance this study is to be based on
    /// </summary>
    public string? SopInstance { get; init; }

    /// <summary>
    /// Specify the patient to create this study for
    /// </summary>
    public Guid? PatientIdGuid { get; init; }

    /// <summary>
    /// Set to true to assign the newly created study to the current user
    /// </summary>
    public bool? AssignToSelf { get; init; }

    /// <summary>
    /// Used to create a study on behalf of a different user
    /// </summary>
    public Guid? CreateOnBehalfOf { get; init; }
}