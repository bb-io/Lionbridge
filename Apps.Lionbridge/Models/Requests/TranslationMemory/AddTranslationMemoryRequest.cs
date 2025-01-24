using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Lionbridge.Models.Requests.TranslationMemory;

public class AddTranslationMemoryRequest
{
    [Display("Source native language code")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string SourceNativeLanguageCode { get; set; }

    [Display("Extended metadata keys")]
    public IEnumerable<string>? ExtendedMetadataKeys { get; set; }
    
    [Display("Extended metadata values")]
    public IEnumerable<string>? ExtendedMetadataValues { get; set; }
}