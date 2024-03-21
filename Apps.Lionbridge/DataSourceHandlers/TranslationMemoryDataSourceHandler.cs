using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Requests.SupportAssets;
using Apps.Lionbridge.Models.Responses.TranslationMemory;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.DataSourceHandlers;

public class TranslationMemoryDataSourceHandler : LionbridgeInvocable, IAsyncDataSourceHandler
{
    private readonly string _jobId;

    public TranslationMemoryDataSourceHandler(InvocationContext invocationContext,
        [ActionParameter] GetSupportAssetRequest request) : base(invocationContext)
    {
        _jobId = request.LionBridgeJobId;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(_jobId))
        {
            throw new InvalidOperationException("You should provide a Job ID first");
        }
        
        var endpoint = $"{ApiEndpoints.Jobs}/{_jobId}{ApiEndpoints.TranslationMemories}";
        var request = new LionbridgeRequest(endpoint);

        var response = await Client.ExecuteWithErrorHandling<TranslationMemoriesResponse>(request);

        return response.Embedded.tmupdates.Where(job =>
                context.SearchString == null ||
                job.FileName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(job => job.TmupdateId, job => job.FileName);
    }
}