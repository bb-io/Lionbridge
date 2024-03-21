using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public class TranslationMemoryDto
{
    [JsonProperty("tmupdateId")]
    public string TmupdateId { get; set; }
    
    [JsonProperty("jobId")]
    public string JobId { get; set; }
    
    [JsonProperty("fileId")]
    public string FileId { get; set; }
    
    [JsonProperty("fileType")]
    public string FileType { get; set; }
    
    [JsonProperty("fileName")]
    public string FileName { get; set; }
    
    [JsonProperty("sourceNativeLanguageCode")]
    public string SourceNativeLanguageCode { get; set; }
    
    [JsonProperty("targetNativeLanguageCode")]
    public string TargetNativeLanguageCode { get; set; }
    
    [JsonProperty("extendedMetadata")]
    public Dictionary<string, string> ExtendedMetadata { get; set; }
}