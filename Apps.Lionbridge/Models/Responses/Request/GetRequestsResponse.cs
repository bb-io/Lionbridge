using Apps.Lionbridge.DataSourceHandlers;
using Apps.Lionbridge.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System.ComponentModel;

namespace Apps.Lionbridge.Models.Responses.Request;

public class GetRequestsResponse
{
    public IEnumerable<RequestDto> Requests { get; set; }

    [Display("All in review?", Description = "True if the status of all requests is in review and the target files can be downloaded")]
    public bool AreAllRequestsCompleted { get => Requests.All(x => x.StatusCode == "REVIEW_TRANSLATION"); }

    [Display("Request IDs", Description = "A list of only the IDs of the requests. Useful to map to actions that require a list of IDs")]
    public IEnumerable<string> RequestIds { get => Requests.Select(x => x.RequestId); }
}