using Apps.Lionbridge.DataSourceHandlers;
using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Webhooks.Inputs;

public class JobStatusUpdatedInput
{
    [Display("Status codes"), DataSource(typeof(JobStatuses))]
    public IEnumerable<string>? StatusCodes { get; set; }

    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string? JobId { get; set; }

    public bool? Archived { get; set; }
    
    public bool? Deleted { get; set; }
}