using System.Collections;

namespace VidiView.Api.DataModel;

/// <summary>
/// The collection of links for a model. 
/// The contained Links dictionary is treated as read only
/// </summary>
[ExcludeFromCodeCoverage]
public class LinkCollection : IDictionary<string, Link>, IEquatable<LinkCollection>
{
    readonly Dictionary<string, Link> _links = new();

    public LinkCollection()
    {
    }

    /// <summary>
    /// Create a link collection
    /// </summary>
    /// <param name="links"></param>
    public LinkCollection(params Link[] links)
    {
        if (links != null)
        {
            foreach (var link in links)
            {
                _links.Add(link.Rel, link);
            }
        }
    }

    /// <summary>
    /// Create a link collection
    /// </summary>
    /// <param name="links"></param>
    public LinkCollection(IEnumerable<Link>? links)
    {
        if (links != null)
        {
            foreach (var link in links)
            {
                _links.Add(link.Rel, link);
            }
        }
    }

    /// <summary>
    /// Return the number of Links in the collection
    /// </summary>
    public int Count => _links.Count;

    /// <summary>
    /// This implementation is hidden to prefer the TryGet extension
    /// </summary>
    /// <param name="rel">Name of link (relation)</param>
    /// <param name="result">The Link object if it exists</param>
    /// <returns>True if the relation e</returns>
    bool IDictionary<string, Link>.TryGetValue(string key, out Link value)
    {
        return _links.TryGetValue(key, out value!);
    }

    /// <summary>
    /// Access to the underlying dictionary
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Link IDictionary<string, Link>.this[string key]
    {
        get => _links[key];
        set
        {
            // When deserializing, the Rel property is not contained
            // in the body of the object, replace with key
            value.Rel = key;
            _links[key] = value;
        }
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

    bool IEquatable<LinkCollection>.Equals(LinkCollection? other)
    {
        if (other == null)
        {
            return false;
        }

        return Enumerable.SequenceEqual(this._links, other._links);
    }
}
