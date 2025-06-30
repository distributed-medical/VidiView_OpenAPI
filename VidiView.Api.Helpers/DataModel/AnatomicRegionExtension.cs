using System.Globalization;
using System.Xml.Linq;
using VidiView.Api.Serialization;

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
        {
            return null!;
        }

        if (region.MapId == Guid.Empty)
        {
            throw new ArgumentException("AnatomicRegion must have a valid MapId", nameof(region));
        }

        var mapElement = new XElement("Map",
            new XAttribute("map", region.MapId.ToString("N", CultureInfo.InvariantCulture))
        );

        if (region.Snomed?.Expression != null)
        {
            mapElement.Add(new XAttribute("snomed", region.Snomed.Expression));
        }

        if (region.Markers?.Length > 0)
        {
            foreach (var marker in region.Markers)
            {
                mapElement.Add(new XElement("Marker",
                    new XAttribute("x", marker.X.ToString(CultureInfo.InvariantCulture)),
                    new XAttribute("y", marker.Y.ToString(CultureInfo.InvariantCulture))
                ));
            }
        }

        if (region.Locations?.Length > 0)
        {
            foreach (var location in region.Locations)
            {
                ArgumentNullException.ThrowIfNullOrEmpty(location, nameof(region.Locations));
                mapElement.Add(new XElement("Location", location));
            }
        }

        return mapElement.ToString(SaveOptions.DisableFormatting);
    }

    /// <summary>
    /// Serialize Anatomic region as Json
    /// </summary>
    /// <param name="region"></param>
    /// <returns></returns>
    public static string ToJson(this AnatomicRegion region)
    {
        if (region == null)
        {
            return null!;
        }

        return System.Text.Json.JsonSerializer.Serialize(region, VidiViewJson.DefaultOptions);
    }

}
