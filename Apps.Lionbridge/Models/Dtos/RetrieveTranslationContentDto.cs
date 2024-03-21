using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public class RetrieveTranslationContentDto
{
    [JsonProperty("requestId")]
    public string RequestId { get; set; }
    
    [JsonProperty("sourceNativeId")]
    public string SourceNativeId { get; set; }
    
    [JsonProperty("sourceNativeLanguageCode")]
    public string SourceNativeLanguageCode { get; set; }
    
    [JsonProperty("targetNativeId")]
    public string TargetNativeId { get; set; }
    
    [JsonProperty("targetNativeLanguageCode")]
    public string TargetNativeLanguageCode { get; set; }
    
    [JsonProperty("targetContent")]
    public List<FieldDto> TargetContent { get; set; }
}