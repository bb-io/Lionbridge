using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Dtos;

public class JobDto
{
    [Display("Job ID")]
    public string JobId { get; set; }
    
    [Display("Job name")]
    public string JobName { get; set; }
    
    [Display("Job description")]
    public string Description { get; set; }
    
    [Display("Status code")]
    public string StatusCode { get; set; }
    
    [Display("Has error")]
    public bool HasError { get; set; }
    
    [Display("Creator ID")]
    public string CreatorId { get; set; }
    
    [Display("Provider ID")]
    public string ProviderId { get; set; }
    
    [Display("Created date")]
    public DateTime CreatedDate { get; set; }
    
    [Display("Modified date")]
    public DateTime ModifiedDate { get; set; }
    
    [Display("Is archived")]
    public bool Archived { get; set; }
    
    [Display("Archive status")]
    public string ArchiveStatus { get; set; }
    
    [Display("Estimated archival date")]
    public DateTime EstimatedArchivalDate { get; set; }
    
    [Display("ShouldQuote")]
    public bool ShouldQuote { get; set; }
    
    [Display("Site ID")]
    public string SiteId { get; set; }
    
    [Display("Global tracking ID")]
    public string GlobalTrackingId { get; set; }
}