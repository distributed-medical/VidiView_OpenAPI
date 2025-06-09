namespace VidiView.Api;

/// <summary>
/// These are well-known study types, that are defined by the manufacturer.
/// These are used to add specific behavior in different applications
/// </summary>
public static class WellKnownStudyType
{
    /// <summary>
    /// Generic study type
    /// </summary>
    public static readonly Guid Generic = new Guid("7F3D4ADE-7AB0-41F3-8549-12262C626989");

    /// <summary>
    /// The study is a mole-mapping study (dermatology)
    /// </summary>
    public static readonly Guid MoleMapping = new Guid("FE71542A-B959-4107-8168-3B049BE3C1C1");

}
