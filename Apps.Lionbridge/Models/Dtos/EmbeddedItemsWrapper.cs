using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public class EmbeddedItemsWrapper<T>
{
    [JsonProperty("_links")]
    public Links Links { get; set; }

    [JsonProperty("_embedded")]
    public T Embedded { get; set; }
}

public class Links
{
    [JsonProperty("next")]
    public Link Next { get; set; }
}

public class Link
{
    [JsonProperty("href")]
    public string Href { get; set; }
}