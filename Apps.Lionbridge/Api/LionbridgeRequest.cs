using RestSharp;

namespace Apps.Lionbridge.Api;

public class LionbridgeRequest : RestRequest
{
    public LionbridgeRequest(string endpoint, Method method = Method.Get) : base(endpoint, method)
    {
    }
}
