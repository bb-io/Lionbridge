
using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Lionbridge.Models.Requests.Job;

public class SearchJobsRequest
{
    [Display("Job name contains")]
    public string? JobName { get; set; }

    [Display("Status")]
    [StaticDataSource(typeof(JobStatusDataHandler))]
    public string? StatusCode { get; set; }

    [Display("Exclude completed")]
    public bool ExcludeCompleted { get; set; }

    [Display("Provider ID")]
    public string? ProviderId { get; set; }

    [Display("Due before")]
    public DateTime? DueBefore { get; set; }

    [Display("Submitted after")]
    public DateTime? SubmittedAfter { get; set; }

    [Display("Submitted before")]
    public DateTime? SubmittedBefore { get; set; }

    [Display("Created after")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before")]
    public DateTime? CreatedBefore { get; set; }

}
