using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Requests.TranslationMemory;

public class AddTranslationMemoryRequest
{
    [Display("Source native language code")]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native language code")]
    public string TargetNativeLanguageCode { get; set; }

    [Display("Extended metadata keys")]
    public IEnumerable<string>? ExtendedMetadataKeys { get; set; }
    
    [Display("Extended metadata values")]
    public IEnumerable<string>? ExtendedMetadataValues { get; set; }
}