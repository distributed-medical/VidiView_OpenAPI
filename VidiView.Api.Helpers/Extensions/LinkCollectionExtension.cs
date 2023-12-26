namespace VidiView.Api.DataModel;

public static class LinkCollectionExtension
{
    /// <summary>
    /// Try to get a specific link
    /// </summary>
    /// <param name="rel">Name of link (relation)</param>
    /// <param name="result">The Link object if it exists</param>
    /// <returns>True if the relation e</returns>
    public static bool TryGet(this LinkCollection? links, string rel, out Link result)
    {
        if (links != null)
        {
            var dict = (IDictionary<string, Link>)links;
            if (dict.TryGetValue(rel, out var r))
            {
                result = r;
                return true;
            }
        }

        result = null!;
        return false;
    }

    /// <summary>
    /// Returns true if the specific link exists
    /// </summary>
    /// <param name="rel"></param>
    /// <returns></returns>
    public static bool Exists(this LinkCollection? links, string rel)
    {
        if (links != null)
        {
            var dict = (IDictionary<string, Link>)links;
            if (dict.TryGetValue(rel, out _))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get a required rel and throw exception if it does not exist
    /// </summary>
    /// <param name="rel"></param>
    /// <returns></returns>
    /// <exception cref="E1021_RelDoesNotExistException"></exception>
    public static Link GetRequired(this LinkCollection? links, string rel)
    {
        if (links.TryGet(rel, out var r))
            return r;

        throw new KeyNotFoundException($"Rel {rel} does not exist");
    }

}
