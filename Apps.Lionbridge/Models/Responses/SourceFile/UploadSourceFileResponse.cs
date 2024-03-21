using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Responses.SourceFile;

public class UploadSourceFileResponse
{
    [JsonProperty("fmsFileId")]
    public string FmsFileId { get; set; }
    
    [JsonProperty("fmsSASToken")]
    public string FmsSASToken { get; set; }
    
    [JsonProperty("fmsUploadUrl")]
    public string FmsUploadUrl { get; set; }
    
    [JsonProperty("fmsPostMultipartUrl")]
    public string FmsPostMultipartUrl { get; set; }
}