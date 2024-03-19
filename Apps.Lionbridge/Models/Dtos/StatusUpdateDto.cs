using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public class StatusUpdateDto
{
    [JsonProperty("updateId")]
    public string UpdateId { get; set; }
    
    [JsonProperty("jobId")]
    public string JobId { get; set; }
    
    [JsonProperty("providerId")]
    public string ProviderId { get; set; }
    
    [JsonProperty("requestIds")]
    public string[] RequestIds { get; set; }
    
    [JsonProperty("statusCode")]
    public string StatusCode { get; set; }
    
    [JsonProperty("updateTime")]
    public string UpdateTime { get; set; }
    
    [JsonProperty("hasError")]
    public bool HasError { get; set; }
    
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }
}