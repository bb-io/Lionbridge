using Newtonsoft.Json;

namespace Apps.Lionbridge.Webhooks.Responses;

public class ListenerDto
{
    [JsonProperty("listenerId")]
    public string ListenerId { get; set; }
    
    [JsonProperty("uri")]
    public string Uri { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("statusCodes")]
    public string[] StatusCodes { get; set; }
    
    [JsonProperty("authType")]
    public string AuthType { get; set; }
    
    [JsonProperty("acknowledgeStatusUpdate")]
    public bool AcknowledgeStatusUpdate { get; set; }
    
    [JsonProperty("disabled")]
    public bool Disabled { get; set; }
}