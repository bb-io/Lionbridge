using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Responses.Job;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.DataSourceHandlers;

public class JobDataSourceHandler(InvocationContext invocationContext)
    : LionbridgeInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, 
        CancellationToken cancellationToken)
    {
        var endpoint = ApiEndpoints.Jobs;
        var request = new LionbridgeRequest(endpoint);
        var response = await Client.ExecuteWithErrorHandling<JobsResponse>(request);
        
        return response.Embedded.Jobs.Where(job =>
            context.SearchString == null ||
            job.JobName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(job => job.JobId, job => job.JobName);
    }
}