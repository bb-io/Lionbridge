using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Lionbridge.Models.Requests.SupportAssets;

public class AddSupportAssetRequest
{
    public string? Description { get; set; }

    [Display("Source native IDs")]
    public IEnumerable<string>? SourceNativeIds { get; set; }
    
    [Display("Source native language")]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native language codes")]
    public IEnumerable<string>? TargetNativeLanguageCodes { get; set; }
    
    [Display("Extended metadata keys")] 
    public IEnumerable<string>? ExtendedMetadataKeys { get; set; }
    
    [Display("Extended metadata values")]
    public IEnumerable<string>? ExtendedMetadataValues { get; set; }
}