using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Responses.Job;
using Apps.Lionbridge.Models.Responses.Request;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.DataSourceHandlers;

public class RequestDataSourceHandler : LionbridgeInvocable, IAsyncDataSourceItemHandler
{
    private readonly string? _jobId;

    public RequestDataSourceHandler(InvocationContext invocationContext, [ActionParameter] GetRequest request) : base(
        invocationContext)
    {
        _jobId = request.JobId;
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_jobId))
        {
            throw new InvalidOperationException("You should input Job ID first");
        }

        var endpoint = $"{ApiEndpoints.Jobs}/{_jobId}{ApiEndpoints.Requests}";
        var request = new LionbridgeRequest(endpoint);
        var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(request);

        return response.Embedded.Requests.Where(job =>
                context.SearchString == null ||
                job.RequestName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(job => new DataSourceItem(job.RequestId, BuildReadableName(job)));
    }

    private string BuildReadableName(RequestDto requestDto)
    {
        return $"{requestDto.RequestName} [{requestDto.TargetNativeLanguageCode}]";
    }
}