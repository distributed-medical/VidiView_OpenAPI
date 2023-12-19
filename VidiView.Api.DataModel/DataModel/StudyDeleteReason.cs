namespace VidiView.Api.DataModel;

public enum StudyDeleteReason
{
    /// <summary>
    /// A user has selected to delete the study. 
    /// </summary>
    /// <remarks>Full access control performed</remarks>
    ManualDeletion = 0,
    
    /// <summary>
    /// Specify this reason if the creation of the study was not intended
    /// </summary>
    /// <remarks>The study will only be deleted if no images or other data has been assigned with the study</remarks>
    UndoCreateStudy = 1, 
}
