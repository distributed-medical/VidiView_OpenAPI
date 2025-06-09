namespace VidiView.Api;

/// <summary>
/// These are well-known media types, that are defined by the manufacturer.
/// These are used to add specific behavior in different applications
/// </summary>
public static class WellKnownMediaType
{
    /// <summary>
    /// This media is a forensic odontology, taken after time of death (post-mortem)
    /// </summary>
    public static readonly Guid ForensicOdontologyPostMortem = new Guid("AD3AAE38-FD10-4811-B96A-EC9C4857FB41");

    /// <summary>
    /// This media is a forensic odontology, taken before time of death (ante-mortem)
    /// </summary>
    public static readonly Guid ForensicOdontologyAnteMortem = new Guid("088457AB-5C54-444F-8ACD-FDE3E7CFE0A2");

}
