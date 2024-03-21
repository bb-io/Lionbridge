using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public class SupportAssetDto
{
    [JsonProperty("supportassetId")] 
    public string SupportAssetId { get; set; }
    
    [JsonProperty("fileId")]
    public string FileId { get; set; }
    
    [JsonProperty("jobId")]
    public string JobId { get; set; }
    
    [JsonProperty("filename")]
    public string Filename { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("sourceNativeIds")]
    public string[] SourceNativeIds { get; set; }
    
    [JsonProperty("sourceNativeLanguageCode")]
    public string SourceNativeLanguageCode { get; set; }
    
    [JsonProperty("targetNativeLanguageCodes")]
    public string[] TargetNativeLanguageCodes { get; set; }
    
    [JsonProperty("createdDate")]
    public DateTime CreatedDate { get; set; }
    
    [JsonProperty("extendedMetadata")]
    public Dictionary<string, string> ExtendedMetadata { get; set; }
}