using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Lionbridge.Models.Requests.Request;

public class UpdateRequestModel
{
    [Display("Request name")]
    public string RequestName { get; set; }
    
    [Display("Source native ID")]
    public string SourceNativeId { get; set; }
    
    [Display("Source native language code")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string? SourceNativeLanguageCode { get; set; }
    
    [Display("Target native language code")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string? TargetNativeLanguageCode { get; set; }
    
    [Display("Target native ID")]
    public string? TargetNativeId { get; set; }

    [Display("Metadata keys", Description = "Metadata keys to be added to the request.")]
    public IEnumerable<string>? MetadataKeys { get; set; }
    
    [Display("Metadata values", Description = "Metadata values to be added to the request.")]
    public IEnumerable<string>? MetadataValues { get; set; }

    [Display("File ID")]
    public string? FileId { get; set; }

    [Display("Source content ID")]
    public string? SourceContentId { get; set; }
}