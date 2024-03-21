using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Responses.SourceFile;

public class UploadFileAsMultipartResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("contentType")]
    public string ContentType { get; set; }
    
    [JsonProperty("size")]
    public long Size { get; set; }
}