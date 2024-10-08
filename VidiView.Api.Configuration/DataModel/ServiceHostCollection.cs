﻿namespace VidiView.Api.Configuration.DataModel;

public class ServiceHostCollection
{
    /// <summary>
    /// Number of items in this collection
    /// </summary>
    public int Count
    {
        get; init;
    }

    /// <summary>
    /// The items
    /// </summary>
    public ServiceHost[] Items => Embedded.Items;

    /// <summary>
    /// Any HAL Rest links associated with this collection
    /// </summary>
    [JsonPropertyName("_links")]
    public Api.DataModel.LinkCollection? Links
    {
        get; init;
    }

    [JsonPropertyName("_embedded")]
    public EmbeddedArray Embedded
    {
        get; init;
    }

    public class EmbeddedArray
    {
        public ServiceHost[] Items
        {
            get; init;
        }
    }
}
