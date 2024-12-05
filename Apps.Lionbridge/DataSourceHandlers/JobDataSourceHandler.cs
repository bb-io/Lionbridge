using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Responses.Job;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.DataSourceHandlers;

public class JobDataSourceHandler(InvocationContext invocationContext)
    : LionbridgeInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, 
        CancellationToken cancellationToken)
    {
        var endpoint = ApiEndpoints.Jobs;
        var request = new LionbridgeRequest(endpoint);
        var response = await Client.ExecuteWithErrorHandling<JobsResponse>(request);
        
        return response.Embedded.Jobs.Where(job =>
            context.SearchString == null ||
            job.JobName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(job => new DataSourceItem(job.JobId, job.JobName));
    }
}