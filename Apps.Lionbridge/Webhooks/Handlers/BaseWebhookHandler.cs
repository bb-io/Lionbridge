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

public abstract class BaseWebhookHandler : BaseInvocable, IWebhookEventHandler
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
        string eventType = GetEventType();
        bridge.Subscribe(eventType, listener.ListenerId, values["payloadUrl"]);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        var listener = await GetListenerIdAsync();
        if (listener != null)
        {
            var bridge = new BridgeService(authenticationCredentialsProviders, _bridgeServiceUrl);
            string eventType = GetEventType();
            bridge.Unsubscribe(eventType, listener.ListenerId, values["payloadUrl"]);
            
            if (!bridge.IsAnySubscriberExist(eventType, listener.ListenerId))
            {
                await DeleteListenerAsync(listener.ListenerId);
            }
        }
    }
    
    private async Task<ListenerDto?> GetListenerIdAsync()
    {
        var request = new LionbridgeRequest(ApiEndpoints.Listeners);
        var response = await Client.ExecuteWithErrorHandling<ListenersResponse>(request);
        
        // Lionbridge api returns eventtype without 'ed' at the end: REQUEST_UPDATED (it will be REQUEST_UPDATE)
        return response.Embedded.listeners.FirstOrDefault(x => SubscriptionEvent.Contains(x.Type));
    }
    
    private async Task<ListenerDto> CreateListenerAsync()
    {
        var request = new LionbridgeRequest(ApiEndpoints.Listeners, Method.Post)
            .AddJsonBody(new
            {
                uri = _bridgeServiceUrl,
                type = SubscriptionEvent,
                statusCodes = GetStatusCodes(),
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

    protected abstract string[] GetStatusCodes();

    private string GetEventType()
    {
        if(SubscriptionEvent.Contains("JOB"))
        {
            return "JOB_UPDATE";
        }
        
        if(SubscriptionEvent.Contains("REQUEST"))
        {
            return "REQUEST_UPDATE";
        }
        
        throw new Exception("Invalid event type");
    }
}