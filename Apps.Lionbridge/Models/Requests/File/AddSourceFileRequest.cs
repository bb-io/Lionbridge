using Apps.Lionbridge.Models.Requests.Request;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Lionbridge.Models.Requests.File;

public class AddSourceFileRequest : AddRequestBaseModel
{
    public FileReference File { get; set; }

    [Display("File name")]
    public string? FileName { get; set; }
    
    [Display("Target native language")]
    public List<string> TargetNativeLanguage { get; set; }
}