namespace VidiView.Api.DataModel;

public static class SettingValueExtension
{
    public static bool GetBool(this SettingValue setting)
    {
        switch (setting.Value)
        {
            case "0":
            case "false":
                return false;
            case "1":
            case "true":
                return true;
            default:
                throw new ArgumentException("The setting value is not of boolean type");
        }
    }

    public static int GetInt(this SettingValue setting)
    {
        if (int.TryParse (setting.Value, out var value))
        {
            return value;
        }

        throw new ArgumentException("The setting value is not of integer type");
    }

    public static string[] GetArray(this SettingValue setting)
    {
        if (string.IsNullOrEmpty( setting.Value))
            return Array.Empty<string>();

        var items = setting.Value.Split(',');

        for (int i = 0; i < items.Length; i++)
        {
            var s = items[i].Trim();
            if (s.Length > 1 && s[0] == '"' && s[s.Length-1] == '"')
            {
                items[i] = s.Substring(1, s.Length - 2).Replace("\"\"", "\"");
            }
            else
            {
                throw new ArgumentException("The setting is not of array type");
            }
        }

        return items;
    }
}
