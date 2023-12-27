using System.Diagnostics;
using System.Text;

namespace VidiView.Api.DataModel;

/// <summary>
/// This class helps with providing parameter values for templated links
/// </summary>
public class TemplatedLink
{
    /// <summary>
    /// Implicit cast from link
    /// </summary>
    /// <param name="link"></param>
    public static implicit operator TemplatedLink(Link link)
    {
        return new TemplatedLink(link);
    }

    /// <summary>
    /// Implicit cast to Uri
    /// </summary>
    /// <param name="link"></param>
    public static explicit operator Uri(TemplatedLink link)
    {
        return new Uri(link.ToUrl());
    }

    readonly Link _link;
    readonly string _hrefWithoutParams;
    readonly ParameterCollection _params;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="link"></param>
    public TemplatedLink(Link link)
    {
        _link = link ?? throw new ArgumentNullException(nameof(link));
        _hrefWithoutParams = link.Href;
        if (link.Templated)
        {
            _params = new ParameterCollection(ref _hrefWithoutParams);
        }
        else
        {
            _params = new ParameterCollection();
        }
    }

    /// <summary>
    /// Provides access to the link parameters
    /// </summary>
    public ParameterCollection Parameters => _params;

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

    /// <summary>
    /// Create Url from the supplied link and any parameters
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown if any required parameter is missing a value</exception>
    public string ToUrl()
    {
        if (_params.Count == 0)
            return _link.Href;

        int offset = 0;
        bool querySeparatorAdded = false;

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

            if (p.StartPos == -1)
            {
                sb.Append(value);
            }
            else
            {
                var startOffset = p.StartPos + offset;
                sb.Insert(startOffset, value);
            }
            offset += value.Length;
        }

        return sb.ToString();
    }
}
