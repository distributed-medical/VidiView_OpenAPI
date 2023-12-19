using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace VidiView.Api.Serialization;

public class VidiViewNamingPolicy : JsonNamingPolicy
{
    readonly ConcurrentDictionary<string, string> _nameCache = new ConcurrentDictionary<string, string>();

    public VidiViewNamingPolicy()
    {
    }

    public override string ConvertName(string name)
    {
        Debug.Assert(name != "Links", "Name should explicitly be set to _links");

        if (_nameCache.TryGetValue(name, out var result))
            return result;

        // Format name with lower case letters separated by hyphens
        var sb = new StringBuilder(32);
        bool followedByLower = false;

        for (int i = 0; i < name.Length; ++i)
        {
            if (char.IsUpper(name[i]))
            {
                if (i < name.Length - 1)
                    followedByLower = char.IsLower(name[i + 1]);

                if (i > 0 && followedByLower)
                    sb.Append('-');
                sb.Append(char.ToLower(name[i]));
            }
            else
            {
                sb.Append(name[i]);
            }
        }
        result = sb.ToString();

        _nameCache.TryAdd(name, result);
        return result;
    }
}
