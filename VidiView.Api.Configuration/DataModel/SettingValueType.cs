using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;

namespace VidiView.Api.Configuration.DataModel;

public enum TimeSpanType
{
    Seconds,
    Minutes,
    Hours,
    Days,
    Months,
    Years,
}

public enum SettingType
{
    Bool,
    String,
    Int,
    Double,
    Array,
}

public class SettingValueType
{
    public SettingValueType()
    {
    }

    public SettingValueType(string typeDef)
    {
        if (typeDef == null)
            throw new ArgumentNullException("No type information has been loaded for this setting");

        try
        {
            var xmlReader = XmlReader.Create(new StringReader(typeDef));
            xmlReader.Read();

            // Check if any other attributes has been assigned
            Unit = xmlReader.GetAttribute("unit");
            var tp = xmlReader.Name;

            if (Enum.TryParse<SettingType>(tp, true, out var type))
            {
                // Ordinary type
                Type = type;
            }
            else if (Enum.TryParse<TimeSpanType>(tp, true, out var timespan))
            {
                // Time span
                Unit = timespan.ToString().ToLower();
                Type = SettingType.Double;
            }
            else if (tp == "enum")
            {
                // Enumeration type
                var enumOptions = new List<SettingDataTypeEnumOption>();
                xmlReader.Read();
                while (xmlReader.Depth > 0)
                {
                    Type = (SettingType)Enum.Parse(typeof(SettingType), xmlReader.Name, true);
                    var descr = xmlReader.GetAttribute("description");
                    var value = xmlReader.ReadElementContentAsString();
                    enumOptions.Add(new SettingDataTypeEnumOption(value, descr));
                }
                Enumeration = enumOptions.ToArray();
            }

        }
        catch (Exception exc)
        {
            throw new ArgumentException("Failed to parse setting data type definition", exc);
        }
    }

    public SettingType Type { get; set; }

    public string Unit { get; set; }

    public SettingDataTypeEnumOption[] Enumeration { get; set; }

    public string Validate(Guid g)
    {
        if (Type != SettingType.String)
            throw new ArgumentException($"A guid value cannot be stored in a {Type} setting");
        return g.ToString();
    }

    public string Validate(int i)
    {
        if (Type != SettingType.Int && Type != SettingType.Double && Type != SettingType.String)
            throw new ArgumentException($"An int value cannot be stored in a {Type} setting");
        return AssertEnumValid(i.ToString());
    }

    public string Validate(double d)
    {
        if (Type != SettingType.Int && Type != SettingType.Double && Type != SettingType.String)
            throw new ArgumentException($"A double value cannot be stored in a {Type} setting");

        if (Type == SettingType.Int)
            if (Math.Round(d, 0) != d)
                throw new ArgumentException($"A double value including decimals cannot be stored in a {Type} setting");

        return d.ToString(CultureInfo.InvariantCulture);
    }

    public string Validate(bool b)
    {
        if (Type != SettingType.Bool)
            throw new ArgumentException($"A bool value cannot be stored in a {Type} setting");
        return b ? "1" : "0";
    }

    public string Validate(string[] arry)
    {
        if (Type != SettingType.Array)
            throw new ArgumentException($"A string[] value cannot be stored in a {Type} setting");
        return StringArray.FromArray(arry).ToString();
    }

    public string Validate(TimeSpan ts)
    {
        if (!IsTimeSpan)
            throw new ArgumentException($"A TimeSpan value cannot be stored in a {Type} setting");
        return GetCount(ts).ToString(CultureInfo.InvariantCulture);
    }

    public string Validate(string value)
    {
        if (value == null)
            throw new ArgumentException(nameof(value));

        // Verify enum value
        value = AssertEnumValid(value);

        // Check if the supplied value is valid for this type
        switch (Type)
        {
            case SettingType.Bool:
                if (int.TryParse(value, out var i))
                    return i != 0 ? "1" : "0";
                else if (bool.TryParse(value, out var b))
                    return b ? "1" : "0";
                break;
            case SettingType.String:
                return value;
            case SettingType.Int:
                if (int.TryParse(value, out var i2))
                    return value;
                break;
            case SettingType.Double:
                if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    return value;
                break;
            case SettingType.Array:
                var p = StringArray.Split(value);
                return value;
            default:
                Debug.Assert(false, "Not implemented");
                break;
        }

        throw new ArgumentException($"The value cannot be stored in a {Type} setting");
    }

    string AssertEnumValid(string value)
    {
        // Check if this is an enumerated value
        if (Enumeration != null)
        {
            var enm = Enumeration.FirstOrDefault((e) => (e.Value.Equals(value, StringComparison.OrdinalIgnoreCase)));
            if (enm == null)
                throw new ArgumentException("The provided value is not among the valid enumeration values");

            value = enm.Value;
        }
        return value;
    }

    /// <summary>
    /// Convert timespan to double, using the specified Unit
    /// </summary>
    /// <param name="ts"></param>
    /// <returns></returns>
    public double GetCount(TimeSpan ts)
    {
        var u = (TimeSpanType)Enum.Parse(typeof(TimeSpanType), Unit, true);
        switch (u)
        {
            case TimeSpanType.Seconds:
                return Math.Round(ts.TotalSeconds, 2);
            case TimeSpanType.Minutes:
                return Math.Round(ts.TotalMinutes, 2);
            case TimeSpanType.Hours:
                return Math.Round(ts.TotalHours, 2);
            case TimeSpanType.Days:
                return Math.Round(ts.TotalDays, 2);
            case TimeSpanType.Months:
                return Math.Round(ts.TotalDays / 30.4166D, 2);
            case TimeSpanType.Years:
                return Math.Round(ts.TotalDays / 365D, 2);
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns true if this represents a time span
    /// </summary>
    [JsonIgnore]
    public bool IsTimeSpan => Enum.TryParse<TimeSpanType>(Unit, true, out _);

    /// <summary>
    /// Parse value including Unit as TimeSpan
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public TimeSpan ParseTimeSpan(string value)
    {
        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
        {
            var ts = (TimeSpanType)Enum.Parse(typeof(TimeSpanType), Unit, true);
            switch (ts)
            {
                case TimeSpanType.Seconds:
                    return TimeSpan.FromSeconds(d);
                case TimeSpanType.Minutes:
                    return TimeSpan.FromMinutes(d);
                case TimeSpanType.Hours:
                    return TimeSpan.FromHours(d);
                case TimeSpanType.Days:
                    return TimeSpan.FromDays(d);
                case TimeSpanType.Months:
                    return TimeSpan.FromDays(d * 30.4166D);
                case TimeSpanType.Years:
                    return TimeSpan.FromDays(d * 365);
                default:
                    throw new ArgumentException($"Unit {Unit} not supported");
            }
        }

        throw new ArgumentException("A double value expected");
    }

    public override string ToString()
    {
        if (Unit != null)
            return $"{Type} {Unit}";
        return Type.ToString();
    }
}

public class SettingDataTypeEnumOption
{
    public SettingDataTypeEnumOption()
    {
    }

    public SettingDataTypeEnumOption(string value, string description)
    {
        Value = value;
        Description = description;
    }

    public string Value { get; set; }

    public string Description { get; set; }

    public override string ToString()
    {
        return Description ?? Value;
    }
}
