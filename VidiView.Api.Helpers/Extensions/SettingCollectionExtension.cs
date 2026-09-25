namespace VidiView.Api.DataModel;

internal static class SettingCollectionExtension
{
    /// <summary>
    /// Update a setting in the local cache
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="value"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public static void UpdateInPlace(this SettingCollection collection, SettingValue value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        for (var i = 0; i < collection.Embedded.Settings.Length; ++i)
        {
            if (collection.Embedded.Settings[i].Key == value.Key)
            {
                collection.Embedded.Settings[i] = value;
                return;
            }
        }

        throw new KeyNotFoundException("The specified setting does not exist");
    }
}
