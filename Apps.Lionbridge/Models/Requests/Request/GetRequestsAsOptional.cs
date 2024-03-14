using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.Request;

public class GetRequestsAsOptional
{
    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string JobId { get; set; }
    
    [Display("Request IDs"), DataSource(typeof(RequestDataSourceHandler))]
    public IEnumerable<string>? RequestIds { get; set; }
}