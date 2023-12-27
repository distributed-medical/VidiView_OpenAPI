namespace VidiView.Api.DataModel;

public class DepartmentCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// The items
    /// </summary>
    public Department[] Items => Embedded.Departments;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection? Links { get; init; }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded { get; init; }

    public class EmbeddedArray
    {
        public Department[] Departments { get; init; }
    }
}
