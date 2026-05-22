using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api.DataModel;

public record Controller
{
    public static implicit operator IdAndName?(Controller? controller)
    {
        return controller == null ? null : new IdAndName(controller.Id, controller.Name);
    }

    /// <summary>
    /// Controller id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Controller name / location
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Controller license serial number
    /// </summary>
    public string SerialNumber { get; init; }

    /// <summary>
    /// Optional asset tag (user assigned)
    /// </summary>
    public string? AssetTag { get; init; }

    /// <summary>
    /// The administrative department this controller belongs to
    /// </summary>
    public IdAndName AdministrativeDepartment { get; init; }

    /// <summary>
    /// The administrative department this controller belongs to
    /// </summary>
    public IdAndName[]? AllowedDepartments { get; init; }

    /// <summary>
    /// Optional notes
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; set; }
}
