using System.Globalization;

namespace VidiView.Api.DataModel;

public static class AnatomicRegionExtension
{
    /// <summary>
    /// Serialize Anatomic region as Xml
    /// </summary>
    /// <param name="region"></param>
    /// <returns></returns>
    public static string ToXml(this AnatomicRegion region)
    {
        if (region == null)
            return null!;

        var ci = CultureInfo.InvariantCulture;

        // Quick and dirty way to create XML value
        return $"<Map Map=\"{region.MapId:N}\" X=\"{region.X.ToString("0.0000000", ci)}\" Y=\"{region.Y.ToString("0.0000000", ci)}\" Snomed=\"{region.Snomed.Expression}\"/>";
    }
}
