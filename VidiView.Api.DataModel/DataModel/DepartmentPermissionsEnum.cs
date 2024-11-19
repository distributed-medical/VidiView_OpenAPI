namespace VidiView.Api.DataModel;

/// <summary>
/// Permissions
/// </summary>
[Flags]
public enum DepartmentPermissions
    : long
{
    /// <summary>
    /// No user access
    /// </summary>
    None = 0x0,

    /// <summary>
    /// List studies in a department
    /// </summary>
    ListStudies = 0x100,

    /// <summary>
    /// Open a study and view snapshots and videos
    /// </summary>
    OpenStudy = 0x200,

    /// <summary>
    /// Edit study information
    /// </summary>
    EditStudy = 0x400,

    /// <summary>
    /// Delete and recover files in studies
    /// </summary>
    DeleteAndRecoverFile = 0x800,

    /// <summary>
    /// Identify/own study
    /// </summary>
    IdentifyStudy = 0x1000,

    /// <summary>
    /// Override a blocked study and view snapshots and videos
    /// </summary>
    OverrideBlockedPatient = 0x2000,

    /// <summary>
    /// Save file from study to local disk
    /// </summary>
    DownloadFile = 0x8000,

    /// <summary>
    /// Create PDF report
    /// </summary>
    CreateReport = 0x10000,

    /// <summary>
    /// View live conference
    /// </summary>
    ViewLiveConference = 0x100000,

    /// <summary>
    /// User can control remote controllable cameras
    /// </summary>
    ControlRcCamera = 0x800000,

    /// <summary>
    /// User can manage VidiView worklist item
    /// </summary>
    ManageVidiViewWorklist = 0x1000000,

    /// <summary>
    /// Display worklist
    /// </summary>
    ViewWorklist = 0x2000000,

    /// <summary>
    /// User can export image to archive
    /// </summary>
    ExportImage = 0x4000000,

    /// <summary>
    /// User can upload/import file
    /// </summary>
    ImportFile = 0x8000000,

    /// <summary>
    /// This department is available in the VidiView Capture app
    /// </summary>
    UseWithVidiViewCapture = 0x10000000,

    /// <summary>
    /// This department is available in the VidiView Client app
    /// </summary>
    UseWithVidiViewClient = 0x20000000,

    /// <summary>
    /// This department is available in the WebView Client app
    /// </summary>
    UseWithWebViewClient = 0x40000000,

    /// <summary>
    /// Manually delete study
    /// </summary>
    DeleteIdentifiedStudy = 0x100000000,

    /// <summary>
    /// Undo deletion of a study
    /// </summary>
    UndoDeleteStudy = 0x200000000,

}