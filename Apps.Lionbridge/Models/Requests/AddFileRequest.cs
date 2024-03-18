using Apps.Lionbridge.Models.Requests.File;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Lionbridge.Models.Requests;

public class AddFileRequest
{
    public FileReference File { get; set; }

    [Display("File name")]
    public string? FileName { get; set; }
    
    [Display("Target native language")]
    public string TargetNativeLanguage { get; set; }

    public AddFileRequest()
    { }
    
    public AddFileRequest(AddSourceFileRequest request)
    {
        File = request.File;
        FileName = request.FileName;
        TargetNativeLanguage = request.TargetNativeLanguage;
    }
}