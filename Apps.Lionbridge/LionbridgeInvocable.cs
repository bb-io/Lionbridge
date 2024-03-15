using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge;

public class LionbridgeInvocable : BaseInvocable
{
    protected readonly LionbridgeClient Client;

    protected LionbridgeInvocable(InvocationContext invocationContext) : base(invocationContext) 
    {
        Client = new(InvocationContext.AuthenticationCredentialsProviders);
    }
    
    protected async Task<RequestDto> GetRequest(string jobId, string requestId)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}" +
                                               $"{ApiEndpoints.Requests}/{requestId}");
        return await Client.ExecuteWithErrorHandling<RequestDto>(apiRequest);
    }
}