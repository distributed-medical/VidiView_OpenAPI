using System.Collections;

namespace VidiView.Api.DataModel;

/// <summary>
/// The collection of links for a model. This is a struct, ensuring it is never null. 
/// The contained Links dictionary is treated as read only
/// </summary>
public struct LinkCollection : IDictionary<string, Link>
{
    Dictionary<string, Link>? _links;
    bool _isReadOnly;

    /// <summary>
    /// Try to get a specific link
    /// </summary>
    /// <param name="rel">Name of link (relation)</param>
    /// <param name="result">The Link object if it exists</param>
    /// <returns>True if the relation e</returns>
    public bool TryGet(string rel, out Link result)
    {
        _isReadOnly = true;
        if (PrivateDict.TryGetValue(rel, out var r))
        {
            result = r;
            return true;
        }
        else
        {
            result = null!;
            return false;
        }
    }

    /// <summary>
    /// Returns true if the specific link exists
    /// </summary>
    /// <param name="rel"></param>
    /// <returns></returns>
    public bool Exists(string rel)
    {
        _isReadOnly = true;
        return PrivateDict.TryGetValue(rel, out _);
    }

    /// <summary>
    /// Get a required rel and throw exception if it does not exist
    /// </summary>
    /// <param name="rel"></param>
    /// <returns></returns>
    /// <exception cref="E1021_RelDoesNotExistException"></exception>
    public Link GetRequired(string rel)
    {
        _isReadOnly = true;
        if (PrivateDict.TryGetValue(rel, out var result))
            return result;

        throw new KeyNotFoundException($"Rel {rel} does not exist");
    }

    /// <summary>
    /// Return the number of Links in the collection
    /// </summary>
    public int Count => PrivateDict.Count;

    /// <summary>
    /// Get the private dict (creates it if it's null)
    /// </summary>
    private Dictionary<string, Link> PrivateDict {
        get
        {
            _links ??= new Dictionary<string, Link>();
            return _links;
        }
    }

    #region Deserialization support
    bool ICollection<KeyValuePair<string, Link>>.IsReadOnly => _isReadOnly;

    Link IDictionary<string, Link>.this[string key] {
        get => PrivateDict[key];
        set => PrivateDict[key] = value;
    }
    #endregion

    #region Implementation of Read-only Dictionary
    void IDictionary<string, Link>.Add(string key, Link value)
    {
        throw new NotSupportedException();
    }

    bool IDictionary<string, Link>.ContainsKey(string key)
    {
        return PrivateDict.ContainsKey(key);
    }

    bool IDictionary<string, Link>.Remove(string key)
    {
        throw new NotSupportedException();
    }

    bool IDictionary<string, Link>.TryGetValue(string key, out Link value)
    {
        return PrivateDict.TryGetValue(key, out value);
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
        throw new NotImplementedException();
    }

    void ICollection<KeyValuePair<string, Link>>.CopyTo(KeyValuePair<string, Link>[] array, int arrayIndex)
    {
        throw new NotSupportedException();
    }

    bool ICollection<KeyValuePair<string, Link>>.Remove(KeyValuePair<string, Link> item)
    {
        throw new NotSupportedException();
    }

    ICollection<string> IDictionary<string, Link>.Keys => PrivateDict.Keys;

    ICollection<Link> IDictionary<string, Link>.Values => PrivateDict.Values;
    #endregion

    IEnumerator<KeyValuePair<string, Link>> IEnumerable<KeyValuePair<string, Link>>.GetEnumerator()
    {
        return PrivateDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return PrivateDict.GetEnumerator();
    }

}
