namespace VidiView.Api.DataModel;

[JsonConverter(typeof(StringEnumConverterEx<ADDomainType>))]
public enum ADDomainType
{
    ForestMember = 0x1,
    ForestTrust = 0x2,
    DomainTrust = 0x4
}

public record ADDomainObject
{
    /// <summary>
    /// Domain name
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Type
    /// </summary>
    public ADDomainType Type { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ADDomainObject[]? Children { get; init; }

    public override string ToString()
    {
        return Name;
    }
}