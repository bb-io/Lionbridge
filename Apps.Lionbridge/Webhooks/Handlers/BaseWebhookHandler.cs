using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Webhooks.Bridge;
using Apps.Lionbridge.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.Lionbridge.Webhooks.Handlers;

public class BaseWebhookHandler : BaseInvocable, IWebhookEventHandler
{
    private readonly string _bridgeServiceUrl;
    
    private string SubscriptionEvent { get; set; }
    
    protected readonly LionbridgeClient Client;

    public BaseWebhookHandler(InvocationContext invocationContext, string subEvent) : base(invocationContext)
    {
        SubscriptionEvent = subEvent;
        _bridgeServiceUrl = $"{invocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/lionbridge";
        Client = new LionbridgeClient(invocationContext.AuthenticationCredentialsProviders);
    }


    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        var listener = await GetListenerIdAsync();
        if (listener == null)
        {
            listener = await CreateListenerAsync();
        }
        
        var bridge = new BridgeService(authenticationCredentialsProviders, _bridgeServiceUrl);
        bridge.Subscribe(SubscriptionEvent, listener.ListenerId, values["payloadUrl"]);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        var listener = await GetListenerIdAsync();
        if (listener != null)
        {
            var bridge = new BridgeService(authenticationCredentialsProviders, _bridgeServiceUrl);
            bridge.Unsubscribe(SubscriptionEvent, listener.ListenerId, values["payloadUrl"]);
            await DeleteListenerAsync(listener.ListenerId);
        }
    }
    
    private async Task<ListenerDto?> GetListenerIdAsync()
    {
        var request = new LionbridgeRequest(ApiEndpoints.Listeners);
        var response = await Client.ExecuteWithErrorHandling<ListenersResponse>(request);
        
        return response.Embedded.Listeners.FirstOrDefault(x => x.Type == SubscriptionEvent);
    }
    
    private async Task<ListenerDto> CreateListenerAsync()
    {
        var request = new LionbridgeRequest(ApiEndpoints.Listeners, Method.Post)
            .AddJsonBody(new
            {
                uri = _bridgeServiceUrl,
                type = SubscriptionEvent,
                statusCodes = new []
                {
                    "IN_TRANSLATION"
                },
                acknowledgeStatusUpdate = true
            });
        
        var response = await Client.ExecuteWithErrorHandling<ListenerDto>(request);
        return response;
    }

    private async Task DeleteListenerAsync(string listenerId)
    {
        var request = new LionbridgeRequest($"{ApiEndpoints.Listeners}/{listenerId}", Method.Delete);
        await Client.ExecuteWithErrorHandling(request);
    }
}