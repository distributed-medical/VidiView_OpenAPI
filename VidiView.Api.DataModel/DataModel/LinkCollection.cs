using System.Collections;

namespace VidiView.Api.DataModel;

/// <summary>
/// The collection of links for a model. This is a struct, ensuring it is never null. 
/// The contained Links dictionary is treated as read only
/// </summary>
public class LinkCollection : IDictionary<string, Link>
{
    readonly Dictionary<string, Link> _links = new();

    /// <summary>
    /// Return the number of Links in the collection
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// This implementation is hidden to prefer the TryGet extension
    /// </summary>
    /// <param name="rel">Name of link (relation)</param>
    /// <param name="result">The Link object if it exists</param>
    /// <returns>True if the relation e</returns>
    bool IDictionary<string, Link>.TryGetValue(string key, out Link value)
    {
        return _links.TryGetValue(key, out value);
    }

    /// <summary>
    /// Access to the underlying dictionary
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Link IDictionary<string, Link>.this[string key] {
        get => _links[key];
        set => _links[key] = value;
    }

    #region Implementation of Read-only Dictionary
    bool ICollection<KeyValuePair<string, Link>>.IsReadOnly => false;


    void IDictionary<string, Link>.Add(string key, Link value)
    {
        throw new NotSupportedException();
    }

    bool IDictionary<string, Link>.ContainsKey(string key)
    {
        return _links.ContainsKey(key);
    }

    bool IDictionary<string, Link>.Remove(string key)
    {
        throw new NotSupportedException();
    }

    void ICollection<KeyValuePair<string, Link>>.Add(KeyValuePair<string, Link> item)
    {
        throw new NotSupportedException();
    }

    void ICollection<KeyValuePair<string, Link>>.Clear()
    {
        throw new NotSupportedException();
    }

    bool ICollection<KeyValuePair<string, Link>>.Contains(KeyValuePair<string, Link> item)
    {
        return _links.Contains(item);
    }

    void ICollection<KeyValuePair<string, Link>>.CopyTo(KeyValuePair<string, Link>[] array, int arrayIndex)
    {
        throw new NotSupportedException();
    }

    bool ICollection<KeyValuePair<string, Link>>.Remove(KeyValuePair<string, Link> item)
    {
        throw new NotSupportedException();
    }

    ICollection<string> IDictionary<string, Link>.Keys => _links.Keys;

    ICollection<Link> IDictionary<string, Link>.Values => _links.Values;
    #endregion

    IEnumerator<KeyValuePair<string, Link>> IEnumerable<KeyValuePair<string, Link>>.GetEnumerator()
    {
        return _links.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _links.GetEnumerator();
    }
}
