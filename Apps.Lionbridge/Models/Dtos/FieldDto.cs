using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public class FieldDto
{
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}