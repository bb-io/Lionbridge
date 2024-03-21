using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.Request;

public class GetRequest
{
    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string JobId { get; set; }
    
    [Display("Request ID"), DataSource(typeof(RequestDataSourceHandler))]
    public string RequestId { get; set; }
}