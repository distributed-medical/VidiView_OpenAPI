namespace VidiView.Api.DataModel;

public static class LinkCollectionExtension
{
    /// <summary>
    /// Try to get a specific link. This is used instead
    /// of the default TryGetValue implementation to easily
    /// support the situation where the LinkCollection is null
    /// </summary>
    /// <param name="rel">Name of link (relation)</param>
    /// <param name="result">The Link object if it exists</param>
    /// <returns>True if the relation e</returns>
    public static bool TryGet(this LinkCollection? links, string rel, out Link value)
    {
        var dict = (IDictionary<string, Link>?)links;
        value = null!;
        return dict?.TryGetValue(rel, out value) == true;
    }

    /// <summary>
    /// Returns true if the specific link exists
    /// </summary>
    /// <param name="rel"></param>
    /// <returns></returns>
    public static bool Exists(this LinkCollection? links, string rel)
    {
        var dict = (IDictionary<string, Link>?)links;
        return dict?.ContainsKey(rel) == true;
    }

    /// <summary>
    /// Get a required rel and throw exception if it does not exist
    /// </summary>
    /// <param name="rel"></param>
    /// <returns></returns>
    /// <exception cref="E1021_RelDoesNotExistException"></exception>
    public static Link GetRequired(this LinkCollection? links, string rel)
    {
        if (links?.TryGet(rel, out var r) == true)
            return r;

        throw new KeyNotFoundException($"Rel {rel} does not exist");
    }
}
