using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Dtos;

public class UpdateJobApiRequest
{
    [Display("Job name")]
    public string? JobName { get; set; }
    
    public string? Description { get; set; }

    [Display("PO reference")]
    public string? PoReference { get; set; }

    [Display("Due date")]
    public string? DueDate { get; set; }

    [Display("Custom data")]
    public string? CustomData { get; set; }

    [Display("Should quote")]
    public bool? ShouldQuote { get; set; }
    
    [Display("Provider ID")]
    [DataSource(typeof(ProviderDataSourceHandler))]
    public string? ProviderId { get; set; }

    [Display("Connector name")]
    public string? ConnectorName { get; set; }

    [Display("Connector version")]
    public string? ConnectorVersion { get; set; }

    [Display("Service type")]
    public string? ServiceType { get; set; }

    [Display("Extended metadata")]
    public Dictionary<string,string> ExtendedMetadata { get; set; }
    
    [Display("Labels")]
    public Dictionary<string,string> Labels { get; set; }
}