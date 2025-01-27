using Apps.Lionbridge.DataSourceHandlers;
using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Webhooks.Inputs;

public class JobStatusUpdatedInput
{
    [Display("Status codes"), StaticDataSource(typeof(JobStatusDataHandler))]
    public IEnumerable<string>? StatusCodes { get; set; }

    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string? JobId { get; set; }

    [Display("Job name contains")]
    public string? JobName { get; set; }

    public bool? Archived { get; set; }
    
    public bool? Deleted { get; set; }
}