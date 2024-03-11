using Apps.Lionbridge.Api;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Responses.Provider;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.DataSourceHandlers;

public class ProviderDataSourceHandler : LionbridgeInvocable, IAsyncDataSourceHandler
{
    public ProviderDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, 
        CancellationToken cancellationToken)
    {
        var endpoint = "/providers?includeDeletedOnes=false";
        var providers = new List<ProviderDto>();
        string? nextToken;

        do
        {
            var request = new LionbridgeRequest(endpoint);
            var response = await Client.ExecuteWithErrorHandling<ProvidersResponse>(request);

            providers.AddRange(response.Embedded.Providers.Where(provider =>
                context.SearchString == null ||
                provider.ProviderName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase)));

            nextToken = response.Next;
            endpoint = $"/providers?includeDeletedOnes=false&next={nextToken}";
        } while (nextToken != null);

        return providers.ToDictionary(provider => provider.ProviderId, provider => provider.ProviderName);
    }
}