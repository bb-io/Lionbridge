using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.Job;

public class GetJobRequest
{
    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string JobId { get; set; }
}