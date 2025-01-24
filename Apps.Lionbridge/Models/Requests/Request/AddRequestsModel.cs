using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Lionbridge.Models.Requests.Request;

public class AddRequestsModel
{
    [Display("Request name")]
    public string RequestName { get; set; }
    
    [Display("Source native ID")]
    public string SourceNativeId { get; set; }
    
    [Display("Source native language")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native IDs")]
    public IEnumerable<string>? TargetNativeIds { get; set; }
    
    [Display("Target native languages")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string> TargetNativeLanguageCodes { get; set; }
    
    [Display("Word count")]
    public int? WordCount { get; set; } = 0;
    
    [Display("Field names")]
    public IEnumerable<string>? FieldNames { get; set; }
    
    [Display("Field values")]
    public IEnumerable<string>? FieldValues { get; set; }
    
    [Display("Field comments")]
    public IEnumerable<string>? FieldComments { get; set; }
}