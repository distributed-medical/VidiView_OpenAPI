using System.Collections;
using System.Text;

namespace VidiView.Api.Configuration.DataModel;

public class StringArray : IEnumerable<string>
{
    /// <summary>
    /// Split a concatenated string
    /// </summary>
    /// <param name="concatenated"></param>
    /// <returns></returns>
    public static StringArray Split(string concatenated)
    {
        var result = new StringArray();
        result.SplitInternal(concatenated);
        return result;
    }

    /// <summary>
    /// Build a string array from input strings
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static StringArray FromArray(params string[] values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values));

        var result = new StringArray();
        result.AddRange(values);
        return result;
    }

    /// <summary>
    /// Build a string array from input strings
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static StringArray FromArray(IEnumerable<string> values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values));

        var result = new StringArray();
        result.AddRange(values);
        return result;
    }

    readonly List<string> _arry = new List<string>();

    public StringArray()
    {
    }

    public int Count => _arry.Count;

    /// <summary>
    /// Add a range of values
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="ArgumentNullException"/>
    public void AddRange(IEnumerable<string> values)
    {
        foreach (var value in values)
            Add(value);
    }

    /// <summary>
    /// Add a string to the array
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentNullException"/>
    public void Add(string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        _arry.Add(value);
    }

    /// <summary>
    /// Remove the first occurance of a string from the array
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Remove(string value)
    {
        return _arry.Remove(value);
    }

    /// <summary>
    /// Concatenate array of strings to single string with each value in quotes
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (_arry.Count == 0)
            return "";

        var sb = new StringBuilder();
        foreach (string s in _arry)
        {
            if (s != null)
            {
                sb.Append("\"");
                sb.Append(s.Replace("\"", "\"\""));
                sb.Append("\", ");
            }
        }

        if (sb.Length > 2)
            sb.Length -= 2; // Remove trailing comma and space

        return sb.ToString();
    }

    public string[] ToArray()
    {
        return _arry.ToArray();
    }

    public IEnumerator<string> GetEnumerator()
    {
        return ((IEnumerable<string>)_arry).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_arry).GetEnumerator();
    }

    void SplitInternal(string value)
    {
        int start = 0;
        bool insideQuotes = false;

        if (value == null)
            return;

        // Iterate each char
        for (int i = 0; i < value.Length; ++i)
        {
            if (value[i] == '"')
            {
                // Check if next char is also double quotes (escape sequence)
                if (i < value.Length - 1 && value[i + 1] == '"')
                {
                    // Escaped!
                    ++i;
                    continue;
                }

                // Invert quotes
                insideQuotes = !insideQuotes;
            }
            else if (value[i] == ',' && !insideQuotes)
            {
                _arry.Add(Unquote(value.Substring(start, i - start).Trim()));
                start = i + 1;
            }
            else if (value[i] != ' ' && !insideQuotes)
            {
                throw new ArgumentException("A quoted, comma separated string is expected");
            }
        }

        if (start < value.Length && !insideQuotes)
            _arry.Add(Unquote(value.Substring(start).Trim()));
    }

    static string Unquote(string value)
    {
        if (value.StartsWith("\"") && value.EndsWith("\""))
            value = value.Substring(1, value.Length - 2);

        return value.Replace("\"\"", "\"");
    }
}
