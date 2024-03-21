using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Dtos;

public class RequestDto
{
    [Display("Request ID")]
    public string RequestId { get; set; }
    
    [Display("Job ID")]
    public string JobId { get; set; }
    
    [Display("Status code")]
    public string StatusCode { get; set; }
    
    [Display("Has error")]
    public bool HasError { get; set; }
    
    [Display("Target native language")]
    public string TargetNativeLanguageCode { get; set; }
    
    [Display("Created date")]
    public DateTime CreatedDate { get; set; }
    
    [Display("Modified date")]
    public DateTime ModifiedDate { get; set; }
    
    [Display("Word count")]
    public int WordCount { get; set; }
    
    [Display("Request name")]
    public string RequestName { get; set; }
    
    [Display("Source native ID")]
    public string SourceNativeId { get; set; }
    
    [Display("Source native language")]
    public string SourceNativeLanguageCode { get; set; }

    [Display("File name")]
    public string? FileName { get; set; }

    [Display("File type")]
    public string? FileType { get; set; }

    [Display("File ID")]
    public string? FileId { get; set; }

    [Display("Source content ID")]
    public string? SourceContentId { get; set; }
}