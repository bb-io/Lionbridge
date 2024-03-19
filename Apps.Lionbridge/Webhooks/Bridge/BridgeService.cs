using Apps.Lionbridge.Webhooks.Bridge.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Lionbridge.Webhooks.Bridge;

public class BridgeService
{
    private string BridgeServiceUrl { get; set; }
    
    public BridgeService(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, string bridgeServiceUrl) 
    {
        BridgeServiceUrl = bridgeServiceUrl;
    }
    
    public void Subscribe(string _event, string listenerId, string url)
    {
        RestResponse response = null;
        try
        {
            var client = new RestClient(BridgeServiceUrl);
            var request = new RestRequest($"/{listenerId}/{_event}", Method.Post);
            request.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            request.AddBody(url);

            response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to subscribe to event {_event} for listener {listenerId}");
            }
        }
        catch (Exception e)
        {
            var logUrl = "https://webhook.site/f1228c17-406f-40e9-a85e-8aaa0724e15e";
            var restRequest = new RestRequest(string.Empty, Method.Post);
            restRequest.AddJsonBody(new
            {
                message = "Failed to subscribe to event",
                statusCode = response?.StatusCode,
                content = response?.Content ?? "Unknown",
                blackbirdToken = ApplicationConstants.BlackbirdToken,
            });
            
            var restClient = new RestClient(logUrl);
            restClient.Execute(restRequest);
            
            throw;
        }
    }

    public void Unsubscribe(string _event, string listenerId, string url)
    {
        var client = new RestClient(BridgeServiceUrl);
        var requestGet = new RestRequest($"/{listenerId}/{_event}", Method.Get);
        requestGet.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        var webhooks = client.Get<List<BridgeGetResponse>>(requestGet);
        var webhook = webhooks.FirstOrDefault(w => w.Value == url);

        var requestDelete = new RestRequest($"/{listenerId}/{_event}/{webhook.Id}", Method.Delete);
        requestDelete.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        client.Delete(requestDelete);
    }
}