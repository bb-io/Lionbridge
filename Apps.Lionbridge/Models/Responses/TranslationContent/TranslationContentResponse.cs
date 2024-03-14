using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Responses.TranslationContent;

public class TranslationContentResponse
{
    [JsonProperty("sourcecontentId")]
    public string SourceContentId { get; set; }
    
    [JsonProperty("fields")]
    public List<Field> Fields { get; set; }
}

public class Field
{
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}