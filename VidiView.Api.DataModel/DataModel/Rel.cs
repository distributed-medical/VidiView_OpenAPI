namespace VidiView.Api.DataModel;

public static class Rel
{
    public const string ActiveSessions = "active-sessions";
    public const string ActiveSync = "active-sync";
    public const string AddAnnotation = "add-annotation";
    public const string AddFileToExportQueue = "add-for-export";
    public const string AddStudy = "add-study";
    public const string AllStudies = "all-studies";
    public const string AnnounceUpload = "announce-upload";
    public const string Assign = "assign";
    public const string AssignedStudies = "assigned-studies";
    public const string AuditEvents = "audit-events";
    public const string AuditLogForPatient = "audit-log-for-patient";
    public const string AuditLogForStudy = "audit-log-for-study";
    public const string AuditLogForUser = "audit-log-for-user";
    public const string AuditLogLatestEntries = "audit-log-latest";
    public const string AuditLogPatientInteractions = "audit-log-patient-interactions";
    public const string AuthenticatePassword = "authenticate-username-password";
    public const string AuthenticatePin = "authenticate-username-pincode";
    public const string AuthenticatePinEnabled = "authenticate-pin-enabled";
    public const string AuthenticateWindows = "authenticate-windows";
    public const string AuthenticateX509 = "authenticate-x509";
    public const string AuthenticateX509Windows = "authenticate-x509-windows";
    public const string AuthenticateToken = "authenticate-login-token";
    public const string BiometricLoginToken = "biometric-login-token";
    public const string CameraCommand = "camera-command";
    public const string ClientDeviceRegistration = "device-registration";
    public const string Clear = "clear";
    public const string Close = "close";
    public const string Conferences = "conferences";
    public const string ConferenceSources = "conference-sources";
    public const string Configuration = "configuration";
    public const string Contents = "contents";
    public const string Controllers = "controllers";
    public const string Create = "create";
    public const string CreatePatient = "create-patient";
    public const string CreateScheduledStudy = "create-scheduled-study";
    public const string CreateStudy = "create-study";
    public const string Delete = "delete";
    public const string DeleteDevice = "delete-device";
    public const string DeleteOverride = "delete-override";
    public const string DeletedFiles = "deleted-files";
    public const string Departments = "departments";
    public const string DicomMetadata = "dicom-metadata";
    public const string Download = "download";
    public const string ScheduledStudies = "scheduled-studies";
    public const string UnidentifiedStudies = "unidentified-studies";
    public const string Enclosure = "enclosure";
    public const string ExportStatus = "export-status";
    public const string ExportQueues = "export-queues";
    public const string ExtractFrame = "extract-frame";
    public const string Favourite = "favourite";
    public const string File = "file";
    public const string Files = "files";
    public const string FindPatient = "find-patient";
    public const string FindStudy = "find-study";
    public const string GrantDevice = "grant-device";
    public const string IdentifyStudy = "identify";

    [Obsolete("Use File instead", true)]
    public const string Image = "image";
    [Obsolete("Use Files instead", true)]
    public const string Images = "images";
    [Obsolete("Use POST RequestToken instead", false)]
    public const string IssueSamlToken = "issue-saml-token";

    public const string JoinAudioLegacy = "join-av-legacy";
    public const string LeaveAudioLegacy = "leave-av-legacy";
    public const string Load = "load";
    public const string LookupPatient = "lookup-patient";
    public const string MimeTypes = "mime-types";
    public const string MyPersonalWorklists = "personal-worklists";
    public const string MyRecentStudies = "recent-studies";
    public const string MyActiveStudies = "active-sync";
    public const string ParseSnomed = "parse-snomed";
    public const string Patient = "patient";
    public const string Preferences = "preferences";
    public const string Recover = "recover";
    public const string RegisterClientDevice = "register-device";
    public const string Release = "release";

    [Obsolete("Not to be used anymore", true)]
    public const string ReleaseStudy = "release-study";
    public const string RemoveStudy = "remove-study";
    public const string RequestToken = "request-token";

    [Obsolete("Use POST RequestToken instead", false)]
    public const string RenewSamlToken = "renew-saml-token";

    public const string ReportImage = "report-image";
    public const string Roles = "roles";
    public const string Self = "self";
    public const string ServiceHosts = "service-hosts";
    public const string Settings = "settings";
    public const string Signatures = "signatures";
    public const string Start = "start";
    public const string Studies = "studies";
    public const string Study = "study";
    public const string StudyLock = "study-lock";
    public const string SubscribeLegacy = "subscribe-legacy";
    public const string Thumbnail = "thumbnail";
    public const string Trackable = "trackable";
    public const string Trim = "trim";
    public const string TrustedIssuers = "x509-trusted-issuers";
    public const string UnidentifyStudy = "unidentify";
    public const string UnsubscribeLegacy = "unsubscribe-legacy";
    public const string Update = "update";
    public const string UpdateOverride = "update-override";
    public const string UploadMedia = "upload-media";
    public const string UploadLogFile = "upload-log";
    public const string UserProfile = "user-profile";
    public const string User = "user";
    public const string Users = "users";
    public const string WebSocket = "websocket";
    public const string WorklistMetadata = "worklist-metadata";
    public const string GetLoginPin = "login-pin";
    public const string ClearLoginPin = "clear-login-pin";
    public const string SetLoginPin = "set-login-pin";
    public const string DecodeToken = "decode-token";
}