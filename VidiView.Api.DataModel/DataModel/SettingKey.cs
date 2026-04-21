namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public static class SettingKey
{
    public const string ServerDescription = "Server/Description";

    public const string ServerApiCorsAllowOrigin = "Server/Api/CorsAllowOrigin";
    public const string ServerEnableRemoteConfiguration = "Server/Service/EnableRemoteConfiguration";

    public const string ServerServiceAuthenticationDefaultDomain = "Server/Service/Authentication/AD/DefaultDomain";
    public const string ServerServiceAuthenticationEnableADUsernamePassword = "Server/Service/Authentication/AD/UsernamePassword";
    public const string ServerServiceAuthenticationEnableADSSO = "Server/Service/Authentication/AD/SSO";
    public const string ServerServiceAuthenticationEnableVidiView = "Server/Service/Authentication/VidiView/Enabled";
    public const string ServerServiceAuthenticationVidiViewMinPasswordLength = "Server/Service/Authentication/VidiView/MinPasswordLength";
    public const string ServerServiceAuthenticationVidiViewMaxPasswordAge = "Server/Service/Authentication/VidiView/MaxPasswordAge";
    public const string ServerServiceAuthenticationSectraSharedSecret = "Server/Service/Authentication/ApplicationSSO/SectraSharedSecret";
    public const string ServerServiceAuthenticationTakeCareSharedSecret = "Server/Service/Authentication/ApplicationSSO/TakeCareSharedSecret";
    public const string ServerServiceAuthenticationCareStreamSharedSecret = "Server/Service/Authentication/ApplicationSSO/CareStreamSharedSecret";
    public const string ServerServiceAuthenticationControllerAcceptByDefault = "Server/Service/Authentication/Controller/AcceptByDefault";

    public const string ServerServiceAuthenticationExternalIdPEnabled = "Server/Service/Authentication/ExternalIdP/Enabled";

    public const string ServerServiceAuthenticationX509Enabled = "Server/Service/Authentication/X509/Enabled";
    public const string ServerServiceAuthenticationX509CertificateRevocationCheck = "Server/Service/Authentication/X509/CertificateRevocationCheck";
    public const string ServerServiceAuthenticationX509SmartCardRemovalPolicy = "Server/Service/Authentication/X509/SmartCardRemovalPolicy";
    public const string ServerServiceAuthenticationX509SmartCardReinsertRequirePin = "Server/Service/Authentication/X509/SmartCardReinsertRequirePin";
    public const string ServerServiceAuthenticationX509WindowsUserMapping = "Server/Service/Authentication/X509/WindowsUserMapping";

    public const string ServerServiceAuthorizationAccessTokenLifetime = "Server/Service/Authorization/AccessToken/Lifetime";
    public const string ServerServiceAuthorizationRoleProviderPrefix = "Server/Service/Authorization/RoleProvider/";
    public const string ServerServiceAuthorizationRoleProviderEnabled = "Server/Service/Authorization/RoleProvider/Enabled";
    public const string ServerServiceAuthorizationRoleProviderCustomLibraryPath = "Server/Service/Authorization/RoleProvider/CustomLibraryPath";
    public const string ServerServiceAuthorizationRoleProviderLegacyConfiguration = "Server/Service/Authorization/RoleProvider/LegacyConfiguration";

    public const string ServerServicePort = "Server/Service/Port";
    public const string ServerServiceDisableTls13 = "Server/Service/DisableTls13";
    public const string ServerServiceCertificateHash = "Server/Service/CertificateHash";
    public const string ServerServiceServicePrincipalName = "Server/Service/ServicePrincipalName";
    public const string ServerServiceSniHostName = "Server/Service/SniHostName";
    public const string ServerServiceExternalVisibleName = "Server/Service/ExternalVisibleName";
    public const string ServerServiceEnableCompression = "Server/Service/EnableCompression";

    public const string ServerServiceUnsecureHttpEnabled = "Server/Service/UnsecureHttp/Enabled";
    public const string ServerServiceUnsecureHttpPort = "Server/Service/UnsecureHttp/Port";

    public const string ServerServiceThrottleDownloadRate = "Server/Service/ThrottleDownloadRate";
    public const string ServerServiceThrottleUploadRate = "Server/Service/ThrottleUploadRate";
    public const string ServerServiceMaxConcurrentCalls = "Server/Service/MaxConcurrentCalls";
    public const string ServerServiceMaxConcurrentSessions = "Server/Service/MaxConcurrentSessions";
    public const string ServerServiceMaxPlaybackStreams = "Server/Service/MaxPlaybackStreams";
    public const string ServerServiceMaxVideoRequestBlockSize = "Server/Service/MaxVideoRequestBlockSize";

    public const string ServerLogFailedAuthenticationToEventLog = "Server/Log/EventLog/FailedAuthentication";
    public const string ServerLogTraceSql = "Server/Log/Trace/TraceRawSql";
    public const string ServerLogTraceDebugToFile = "Server/Log/Trace/WriteDebugToFile";
    public const string ServerLogTraceFileRetentionDays = "Server/Log/Trace/FileRetention";
    public const string ServerLogTraceFileAutoSubmit = "Server/Log/Trace/SubmitLogFilesAutomatically";
    public const string ServerLogTraceDetailedDicomLog = "Server/Log/Trace/DetailedDicomLog";

    public const string ServerLogReturnedExceptions = "Server/Log/Trace/ServiceRequestError";

    public const string ServerLogApiRequestEnabled = "Server/Log/ApiRequest/Enabled";
    public const string ServerLogApiRequestLogAllRangeRequests = "Server/Log/ApiRequest/LogAllRangeRequests";
    public const string ServerLogAtnaPrefix = "Server/Log/Atna/";
    public const string ServerLogAtnaEnabled = "Server/Log/Atna/Enabled";
    public const string ServerLogAtnaHost = "Server/Log/Atna/Host";
    public const string ServerLogAtnaPort = "Server/Log/Atna/Port";
    public const string ServerLogAtnaClientCertificate = "Server/Log/Atna/ClientCertificate";
    public const string ServerLogAtnaSourceId = "Server/Log/Atna/SourceId";
    public const string ServerLogAtnaEnterpriseSiteId = "Server/Log/Atna/EnterpriseSiteId";
    public const string ServerLogAtnaSendInterval = "Server/Log/Atna/SendInterval";

    public const string ServerFileArchiveMinimumFileRetention = "Server/ImageFileArchive/MinimumFileRetention";
    public const string ServerFileArchiveKeepUntilLowDiskSpace = "Server/ImageFileArchive/KeepUntilLowDiskSpace";
    public const string ServerFileArchiveLowDiskSpaceCondition = "Server/ImageFileArchive/LowDiskSpaceCondition";
    public const string ServerFileArchiveThumbnailCacheTime = "Server/ImageFileArchive/ThumbnailCacheTime";
    public const string ServerFileArchiveCompressBitmapFilesLossless = "Server/ImageFileArchive/CompressBitmapFilesLossless";

    public const string ServerNotifyEmailSmtpServer = "Server/Notify/Email/SmtpServer";
    public const string ServerNotifyEmailFromAddress = "Server/Notify/Email/FromAddress";

    public const string ServerProxyEnableIPv4 = "Server/UdpProxy/EnableIPv4";
    public const string ServerProxyEnableIPv6 = "Server/UdpProxy/EnableIPv6";
    public const string ServerProxyPoolSize = "Server/UdpProxy/PoolSize";
    public const string ServerProxySocketReceiveBuffer = "Server/UdpProxy/SocketReceiveBuffer";
    public const string ServerProxySocketSendPace = "Server/UdpProxy/SocketSendPace";

    public const string WorkflowProvisioIntegrationServiceSharedSecret = "Workflow/ProvisioIntegrationService/SharedSecret";
    public const string WorkflowDicomCStoreEnabled = "Workflow/DicomCStoreSCP/Enabled";
    public const string WorkflowDicomCStoreEnableUnsecure = "Workflow/DicomCStoreSCP/EnableUnsecure";
    public const string WorkflowDicomCStorePort = "Workflow/DicomCStoreSCP/Port";
    public const string WorkflowDicomCStoreEnableTls = "Workflow/DicomCStoreSCP/EnableTls";
    public const string WorkflowDicomCStoreTlsPort = "Workflow/DicomCStoreSCP/TlsPort";
    public const string WorkflowDicomCStoreLocalAET = "Workflow/DicomCStoreSCP/LocalAETitle";
    public const string WorkflowDicomCStoreMaxAssociations = "Workflow/DicomCStoreSCP/MaxAssociations";
    public const string WorkflowDicomCStoreAllowOverwrite = "Workflow/DicomCStoreSCP/AllowOverwrite";
    public const string WorkflowDicomCStoreFallbackEncoding = "Workflow/DicomCStoreSCP/FallbackEncoding";
    public const string WorkflowDicomCStoreSnomedLanguage = "Workflow/DicomCStoreSCP/SnomedLanguage";

    public const string WorkflowIanEnabled = "Workflow/Ian/Enabled";
    public const string WorkflowIanReceiver = "Workflow/Ian/Receiver";

    public const string WorkflowMllpReceiveV1Enabled = "Workflow/MllpTransport/V1/Enabled";
    public const string WorkflowMllpReceiveV1Port = "Workflow/MllpTransport/V1/Port";
    public const string WorkflowMllpReceiveV1MaxMessageSize = "Workflow/MllpTransport/V1/MaxMessageSize";
    public const string WorkflowMllpReceiveV1MaxConnections = "Workflow/MllpTransport/V1/MaxConcurrentConnections";
    public const string WorkflowMllpReceiveV2Enabled = "Workflow/MllpTransport/V2/Enabled";
    public const string WorkflowMllpReceiveV2Port = "Workflow/MllpTransport/V2/Port";
    public const string WorkflowMllpReceiveV2MaxMessageSize = "Workflow/MllpTransport/V2/MaxMessageSize";
    public const string WorkflowMllpReceiveV2MaxConnections = "Workflow/MllpTransport/V2/MaxConcurrentConnections";

    public const string WorkflowHL7SendingApplication = "Workflow/HL7/SendingApplication";
    public const string WorkflowHL7SendingFacility = "Workflow/HL7/SendingFacility";

    public const string WorkflowHL7MessageLogRetention = "Workflow/HL7/MessageLogRetention";
    public const string WorkflowHL7DefaultCharacterSet = "Workflow/HL7/DefaultCharacterSet";
    public const string WorkflowHL7SpecificAckCharacterSet = "Workflow/HL7/SpecificAckCharacterSet";

    public const string WorkflowTransferQueueManualAddFromUnassignedStudy = "Workflow/TransferQueue/ManualAddFromUnassignedStudy";
    public const string WorkflowTransferQueueExportImmediatelyWhenAssigned = "Workflow/TransferQueue/ExportImmediatelyWhenAssigned";

    public const string PatientPdqEnabled = "System/Patient/Pdq/Enabled";
    public const string PatientPdqType = "System/Patient/Pdq/Type";
    public const string PatientPdqServiceHost = "System/Patient/Pdq/ServiceHost";
    public const string PatientPdqAutoUpdate = "System/Patient/Pdq/AutoUpdate";
    public const string PatientPdqCustomLibraryPath = "System/Patient/Pdq/CustomLibraryPath";
    public const string PatientPdqLegacyConfiguration = "System/Patient/Pdq/LegacyConfiguration";

    public const string PatientMaxReturnedRecords = "System/Patient/MaxReturnRecords";

    public const string PatientManualCreate = "System/Patient/ManualCreate/Enabled";
    public const string PatientAssigningAuthorityId = "System/Patient/ManualCreate/AssigningAuthorityId";
    public const string PatientNamePresentation = "System/Patient/Formatting/PatientName";
    public const string PatientPseudonymizedNamePresentation = "System/Patient/Formatting/PatientName/Pseudonymized";
    public const string PatientIdPresentation = "System/Patient/Formatting/PatientId";
    public const string PatientPseudonymizedIdPresentation = "System/Patient/Formatting/PatientId/Pseudonymized";

    public const string PatientEraseNonReferencedEnabled = "System/Patient/Erase/NonReferenced/Automatic/Enabled";
    public const string PatientEraseNonReferencedAfter = "System/Patient/Erase/NonReferenced/Automatic/After";

    public const string ReportLogotype = "Report/Logotype";
    public const string ReportLogotypeHeight = "Report/LogotypeHeight";
    public const string ReportPageNoFormat = "Report/PageNoFormat";
    public const string ReportRasterDpi = "Report/RasterDpi";
    public const string ReportJpegQuality = "Report/JpegQuality";
    public const string ReportDefaultLanguage = "Report/DefaultLanguage";

    public const string StudyMaxReturnedRecords = "System/Study/MaxReturnRecords";
    public const string StudyListAnyDepartment = "System/Study/List/AllowAnyDepartment";
    public const string StudyListAnyDepartmentForPatient = "System/Study/List/AllowAnyDepartmentForPatient";

    public const string StudyAssignAllowConcurrent = "System/Study/Assign/AllowConcurrent";
    public const string StudyCreateUnscheduled = "System/Study/Create/Unscheduled";
    public const string StudyMediaFavouriteMark = "System/Study/Media/FavouriteMark";
    public const string StudyMediaConvertHevcToAvc = "System/Study/Media/Conversion/ConvertHevcToAvc";
    public const string StudyMediaConvertUnsupportedPhotoToJpeg = "System/Study/Media/Conversion/ConvertUnsupportedPhotoToJpeg";

    public const string StudyDeleteIdentifiedAutomaticallyEnabled = "System/Study/Delete/Identified/Automatic/Enabled";
    public const string StudyDeleteIdentifiedAutomaticallyAfter = "System/Study/Delete/Identified/Automatic/After";
    public const string StudyDeleteExportedAutomaticallyEnabled = "System/Study/Delete/Exported/Automatic/Enabled";
    public const string StudyDeleteExportedAutomaticallyAfter = "System/Study/Delete/Exported/Automatic/After";
    public const string StudyDeleteUnidentifiedAutomaticallyEnabled = "System/Study/Delete/Unidentified/Automatic/Enabled";
    public const string StudyDeleteUnidentifiedAutomaticallyAfter = "System/Study/Delete/Unidentified/Automatic/After";
    public const string StudyDeleteSkipBookmarked = "System/Study/Delete/BlockDeletionOfBookmarkedStudy";

    public const string StudyEraseEmptyEnabled = "System/Study/Erase/Empty/Automatic/Enabled";
    public const string StudyEraseEmptyAfter = "System/Study/Erase/Empty/Automatic/After";

    public const string SystemFieldsStudyAccessionNumber = "System/Fields/Study/AccessionNumber";
    public const string SystemFieldsStudyDepartmentId = "System/Fields/Study/DepartmentId";
    public const string SystemFieldsStudyStudyTypeId = "System/Fields/Study/StudyTypeId";

    public const string WebViewEnable = "WebView/Enable";
    public const string WebViewUseLicenseCertificate = "WebView/UseDefaultCertificate";
    public const string WebViewSessionTimeout = "WebView/SessionTimeout";

    #region VidiView Controller
    public const string ControllerConferenceRemoteCaptureEnable = "Controller/Conference/RemoteCapture/Enable";
    public const string ControllerServiceStudyDeleteEmpty = "Controller/Service/Study/DeleteEmpty";

    #endregion

    #region VidiView Capture
    public const string CaptureServiceEnable = "Capture/Service/Enable";
    public const string CaptureServicePort = "Capture/Service/Port";
    public const string CaptureServiceDeviceAcceptByDefault = "Capture/Service/Device/AcceptByDefault";
    public const string CaptureServiceDeviceDefaultDeviceUse = "Capture/Service/Device/DefaultDeviceUse";
    public const string CaptureServiceAllowPin = "Capture/Service/Authentication/AllowPin";
    public const string CaptureServicePinMinLength = "Capture/Service/Authentication/PinMinimumLength";
    public const string CaptureServicePinMaxInvalidAttempts = "Capture/Service/Authentication/PinMaxInvalidAttempts";

    [Obsolete("No longer in use, in Capture R5")]
    public const string CaptureServiceAllowBiometric = "Capture/Service/Authentication/AllowBiometric";
    [Obsolete("No longer in use, in Capture R5")]
    public const string CaptureServiceBiometricLifetime = "Capture/Service/Authentication/BiometricLifetime";
    [Obsolete("No longer in use, in Capture R5")]
    public const string CaptureServiceBiometricAutoExtend = "Capture/Service/Authentication/BiometricAutoExtend";

    public const string CaptureServiceActiveSyncEnable = "Capture/Service/ActiveSync/Enable";
    public const string CaptureServiceStudyDeleteEmpty = "Capture/Service/Study/DeleteEmpty";

    public const string CaptureOptionsAnatomicMap = "Capture/Options/AnatomicMap";
    public const string CaptureOptionsAnatomicTagging = "Capture/Options/AnatomicTagging";
    public const string CaptureOptionsContinueStudyAllowed = "Capture/Options/ContinueStudyAllowed";
    public const string CaptureOptionsSnapshotResolution = "Capture/Options/SnapshotResolution";
    public const string CaptureOptionsVideoAllowed = "Capture/Options/VideoAllowed";
    public const string CaptureOptionsVideoMaxLength = "Capture/Options/VideoMaxLength";
    public const string CaptureOptionsVideoRecordAudio = "Capture/Options/VideoRecordAudio";
    public const string CaptureOptionsVideoResolution = "Capture/Options/VideoResolution";
    public const string CaptureOptionsVideoCodec = "Capture/Options/VideoCodec";

    public const string CaptureOptionsAllowBiometricCredentials = "Capture/Options/AllowBiometricCredentials";
    public const string CaptureOptionsInactivityTimeout = "Capture/Options/InactivityTimeout";
    public const string CaptureOptionsInactivityInActiveStudyTimeout = "Capture/Options/InactivityInActiveStudyTimeout";
    public const string CaptureOptionsInactivityWarning = "Capture/Options/InactivityWarning";
    #endregion

    #region VidiView Client
    public const string ClientSettingFilter = "Client/"; // Settings that will be returned to the VidiView Client

    public const string ClientDesktopSyncMode = "Client/DesktopSync/Mode";
    public const string ClientDesktopSyncSectraLevel = "Client/DesktopSync/Sectra/Level";
    public const string ClientDesktopSyncSectraAssigningAuthorityId = "Client/DesktopSync/Sectra/AssigningAuthorityId";
    public const string ClientDesktopSyncSectraAccessionNumberGroupId = "Client/DesktopSync/Sectra/AccessionNumberGroupId";
    public const string ClientDesktopSyncSectraExaminationMapping = "Client/DesktopSync/Sectra/ExaminationMapping";
    public const string ClientUIPopupConfirmPatientRelationOnOpenPatient = "Client/UI/Popup/ConfirmPatientRelationOnOpenPatient";
    public const string ClientUIPopupConfirmPatientRelationOnChangeDepartment = "Client/UI/Popup/ConfirmPatientRelationOnChangeDepartment";

    public const string ClientFormattingFilenameSaveImage = "Client/Formatting/Filename/SaveImage";
    public const string ClientFormattingFilenameSaveImagePseudonymized = "Client/Formatting/Filename/SaveImage/Pseudonymized";
    public const string ClientFormattingFilenamePdfReport = "Client/Formatting/Filename/PdfReport";
    public const string ClientFormattingFilenamePdfReportPseudonymized = "Client/Formatting/Filename/PdfReport/Pseudonymized";

    public const string ClientOptionsDisplayLastLogin = "Client/Options/DisplayLastLogin";
    public const string ClientOptionsInactivityTimeout = "Client/Options/InactivityTimeout";
    public const string ClientOptionsInactivityWarning = "Client/Options/InactivityWarning";

    public const string ClientFeaturesPatientDemographicSearch = "Client/Options/Features/PatientDemographicSearch";
    public const string ClientFeaturesPatientPersonalWorklists = "Client/Options/Features/PersonalWorklists";
    public const string ClientFeaturesPatientRecentHistory = "Client/Options/Features/RecentHistory";

    public const string ClientFeaturesPhotoMeasure = "Client/Options/Features/PhotoMeasure";
    public const string ClientFeaturesBatchUpdateMediaFile = "Client/Options/Features/BatchUpdateMediaFile";

    public const string ClientPreferenceLastActingRole = "Client/Preferences/Login/LastActingRole";
    public const string ClientPreferenceUICulture = "Client/Preferences/UI/Culture";
    public const string ClientPreferenceUIThumbnailWidth = "Client/Preferences/UI/StudyView/ThumbnailPaneWidth";
    public const string ClientPreferenceVideoMinimumRequestSize = "Client/Preferences/Video/MinimumRequestSize";

    #endregion

    public const string WebProxyStatus = "WebProxy/UseProxy";
    public const string WebProxyAddress = "WebProxy/Address";
    public const string WebProxyUsername = "WebProxy/Username";
    public const string WebProxyPassword = "WebProxy/Password";

}
