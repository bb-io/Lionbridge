using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Lionbridge.Models.Requests.SupportAssets;

public class AddSupportAssetRequest
{
    public string? Description { get; set; }

    [Display("Source native IDs")]
    public IEnumerable<string>? SourceNativeIds { get; set; }
    
    [Display("Source native language")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native language codes")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string>? TargetNativeLanguageCodes { get; set; }
    
    [Display("Extended metadata keys")] 
    public IEnumerable<string>? ExtendedMetadataKeys { get; set; }
    
    [Display("Extended metadata values")]
    public IEnumerable<string>? ExtendedMetadataValues { get; set; }
}