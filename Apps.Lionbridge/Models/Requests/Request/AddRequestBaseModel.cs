using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Lionbridge.Models.Requests.Request;

public class AddRequestBaseModel
{
    [Display("Request name")]
    public string? RequestName { get; set; }
    
    [Display("Source native ID")]
    public string? SourceNativeId { get; set; }
    
    [Display("Source native language")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native IDs")]
    public IEnumerable<string>? TargetNativeIds { get; set; }
    
    [Display("Word count")]
    public int? WordCount { get; set; } = 0;

    [Display("Metadata keys", Description = "Metadata keys to be added to the request.")]
    public IEnumerable<string>? MetadataKeys { get; set; }
    
    [Display("Metadata values", Description = "Metadata values to be added to the request.")]
    public IEnumerable<string>? MetadataValues { get; set; }
}