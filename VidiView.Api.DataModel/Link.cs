using System.Diagnostics;
using System.Text;

namespace VidiView.Api.DataModel;

/// <summary>
/// Link to other resource (HATEOAS)
/// </summary>
public record Link
{
    string? _hrefWithoutParams;
    ParameterCollection? _params;

    public Link(string? rel, string href)
    {
        Rel = rel;
        Href = href;
    }

    [JsonIgnore]
    public string? Rel { get; init; } 

    /// <summary>
    /// The full uri to the resource
    /// </summary>
    public string Href { get; init; }

    /// <summary>
    /// True if Href is templated (RFC 6570)
    /// </summary>
    public bool Templated { get; init; } 

    /// <summary>
    /// An optional human readable title of the link representation
    /// </summary>
    public string? Title { get; init; } 

    /// <summary>
    /// The content-type to expect when fetching this resource
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// Provides access to any link parameters
    /// </summary>
    [JsonIgnore]
    public ParameterCollection Parameters
    {
        get
        {
            if (_params == null && Templated == true)
                BuildParamsCollection();

            return _params!;
        }
    }

    /// <summary>
    /// Set parameter value, if the parameter exists
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    /// <returns>True if the parameter exists and was set, false otherwise</returns>
    public bool TrySetParameterValue(string parameter, string? value)
    {
        if (!Parameters.TryGet(parameter, out var p))
        {
            Debug.Assert(false, "Is this really expected?");
            return false;
        }
        p.Value = value;
        return true;
    }

    public override string ToString()
    {
        return $"{Rel} {Href}".Trim();
    }

    public Uri ToUri()
    {
        // TODO: Optimize using UriBuilder?
        return new Uri(ToUrl());
    }

    public string ToUrl()
    {
        if (Templated != true)
            return Href;

        int offset = 0;
        bool querySeparatorAdded = false;

        if (_hrefWithoutParams == null)
            BuildParamsCollection();

        var sb = new StringBuilder(_hrefWithoutParams);

        // Add values for each parameter
        offset = 0;
        foreach (var p in Parameters)
        {
            if (p.Value == null) // Ignore this parameter
            {
                if (p.IsPathParam)
                    throw new ArgumentException($"Missing value for path parameter {p.Name}");
                continue;
            }

            string value;

            if (p.IsPathParam)
            {
                Debug.Assert(querySeparatorAdded == false, "Path parameter cannot follow after query parameters");

                // No special treatment is necessary here
                value = Uri.EscapeDataString(p.Value);
            }
            else
            {
                if (!querySeparatorAdded)
                {
                    for (int i = 0; i < p.StartPos; ++i)
                    {
                        if (sb[i] == '?')
                        {
                            querySeparatorAdded = true;
                            break;
                        }
                    }
                }

                value = $"{(querySeparatorAdded ? "&" : "?")}{p.Name}={Uri.EscapeDataString(p.Value)}";
                querySeparatorAdded = true;
            }

            var startOffset = p.StartPos + offset;
            sb.Insert(startOffset, value);
            offset += value.Length;
        }

        return sb.ToString();
    }


    void BuildParamsCollection()
    {
        _hrefWithoutParams = Href;
        _params = new ParameterCollection(ref _hrefWithoutParams);
    }
}
