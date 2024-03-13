using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.Job;

public class UpdateJobRequest
{
    [Display("Job name")] public string? JobName { get; set; }

    public string? Description { get; set; }

    [Display("PO reference")] public string? PoReference { get; set; }

    [Display("Due date")] public DateTime? DueDate { get; set; }

    [Display("Custom data")] public string? CustomData { get; set; }

    [Display("Should quote")] public bool? ShouldQuote { get; set; }

    [Display("Provider ID")]
    [DataSource(typeof(ProviderDataSourceHandler))]
    public string? ProviderId { get; set; }

    [Display("Connector name")] public string? ConnectorName { get; set; }

    [Display("Connector version")] public string? ConnectorVersion { get; set; }

    [Display("Service type")] public string? ServiceType { get; set; }

    [Display("Metadata keys", Description = "Extended metadata keys. For each specified key, a respective value " +
                                            "should be added in the 'Metadata values' input parameter.")]
    public IEnumerable<string>? MetadataKeys { get; set; }

    [Display("Metadata values", Description = "Extended metadata values. For each specified value, a respective " +
                                              "key should be added in the 'Metadata keys' input parameter.")]
    public IEnumerable<string>? MetadataValues { get; set; }
}