using Apps.Lionbridge.DataSourceHandlers;
using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Webhooks.Inputs;

public class GetRequestsInput
{
    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string? JobId { get; set; }
    
    [Display("Request IDs"), DataSource(typeof(RequestDataSourceHandler))]
    public IEnumerable<string>? RequestIds { get; set; }
    
    [Display("Status codes"), DataSource(typeof(RequestStatuses))]
    public IEnumerable<string>? StatusCodes { get; set; }
}