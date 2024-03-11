using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public class EmbeddedItemsWrapper<T>
{
    [JsonProperty("_embedded")]
    public T Embedded { get; set; }
}