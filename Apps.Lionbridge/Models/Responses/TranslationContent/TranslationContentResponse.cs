using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Responses.TranslationContent;

public class TranslationContentResponse
{
    [JsonProperty("sourcecontentId")]
    public string SourceContentId { get; set; }
    
    [JsonProperty("fields")]
    public List<KeyValuePair<string, string>> Fields { get; set; }
}