using System.Collections;
using System.Text;

namespace VidiView.Api.DataModel;

public class ParameterCollection : IEnumerable<ParameterCollection.LinkParam>
{
    public static ParameterCollection Empty { get; } = new ParameterCollection();

    public class LinkParam
    {
        internal LinkParam(string name, int startPos, bool isPath)
        {
            if (name[0] == '?' || name[0] == '&')
                Name = name.Substring(1); // Strip leading query
            else
                Name = name;

            StartPos = startPos;
            IsPathParam = isPath;
        }

        /// <summary>
        /// Is parameter a path parameter. Often an indication that it is required too
        /// </summary>
        public bool IsPathParam { get; }
        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Parameter value
        /// </summary>
        public string? Value { get; set; }

        internal int StartPos { get; }

        public override string ToString()
        {
            return $"{Name} = {Value ?? "null"}";
        }

        internal LinkParam Clone()
        {
            return (LinkParam)this.MemberwiseClone();
        }
    }

    readonly List<LinkParam> _parameters = new();

    private ParameterCollection() : this(Array.Empty <LinkParam>()) 
    {
    }

    public ParameterCollection(ref string template)
    {
        try
        {
            var sb = new StringBuilder(template);
            var list = new List<LinkParam>();

            int paramStartPos = -1;
            bool isPathParameter = true;

            for (int i = 0; i < sb.Length; ++i)
            {
                if (sb[i] == '{')
                    paramStartPos = i;

                else if (sb[i] == '?')
                    isPathParameter = false; // We are now in the query section

                else if (sb[i] == '}')
                {
                    if (paramStartPos < 0)
                        throw new ArgumentException($"Invalid link template: {template}");

                    string paramName = sb.ToString(paramStartPos + 1, i - paramStartPos - 1);
                    int paramLength = i - paramStartPos + 1;

                    if (isPathParameter)
                    {
                        list.Add(new LinkParam(paramName, paramStartPos, isPathParameter));
                    }
                    else
                    {
                        // Check if the parameter really are several parameters concatenated
                        var parts = paramName.Split(',');
                        if (parts.Length == 1)
                        {
                            list.Add(new LinkParam(paramName, paramStartPos, isPathParameter));
                        }
                        else
                        {
                            list.Add(new LinkParam(parts[0], paramStartPos, isPathParameter));

                            for (int j = 1; j < parts.Length; ++j)
                                list.Add(new LinkParam(parts[j], paramStartPos, isPathParameter));

                        }
                    }

                    sb.Remove(paramStartPos, paramLength);
                    i -= paramLength;
                    paramStartPos = -1;
                }

            }

            if (paramStartPos >= 0)
                throw new ArgumentException($"Invalid link template: {template}");

            _parameters = list;
            template = sb.ToString();
        }
        catch (Exception exc)
        {
            throw new Exception("Failed to parse link parameter template", exc);
        }
    }

    private ParameterCollection(IEnumerable<LinkParam> parameters)
    {
        _parameters = new List<LinkParam>(parameters);
    }

    /// <summary>
    /// Return the number of parameters defined
    /// </summary>
    public int Count => _parameters.Count;

    /// <summary>
    /// Return a parameter by its index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public LinkParam this[int index] => _parameters[index];
    
    /// <summary>
    /// Return a parameter by its name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Thrown if the parameter does not exist</exception>
    public LinkParam this[string name] => _parameters.FirstOrDefault((p) => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? throw new NotSupportedException($"The parameter {name} is not defined on this link");

    /// <summary>
    /// Try to return a parameter by its name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGet(string name, out LinkParam value)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        value = _parameters.FirstOrDefault((p) => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
#pragma warning restore CS8601 // Possible null reference assignment.
        return value != null;
    }

    /// <summary>
    /// Update existing parameter, or add a query string parameter if it does not already exist
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void AddOrUpdate(string name, string value)
    {
        var p = _parameters.FirstOrDefault((p) => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (p == null)
        {
            _parameters.Add(new LinkParam(name, -1, false)
            {
                Value = value
            });
        }
        else
        {
            p.Value = value;
        }
    }

    public IEnumerator<LinkParam> GetEnumerator()
    {
        return ((IEnumerable<LinkParam>)_parameters).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_parameters).GetEnumerator();
    }
}
