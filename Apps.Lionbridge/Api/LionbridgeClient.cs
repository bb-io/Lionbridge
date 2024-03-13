using System.Net;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Lionbridge.Api;

public class LionbridgeClient : BlackBirdRestClient
{
    protected override JsonSerializerSettings JsonSettings =>
        new() { MissingMemberHandling = MissingMemberHandling.Ignore };

    public LionbridgeClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        : base(new RestClientOptions
            { ThrowOnAnyError = false, BaseUrl = new Uri("https://content-api.staging.lionbridge.com/v2") })
    {
        var accessToken = GetAccessToken(authenticationCredentialsProviders);
        this.AddDefaultHeader("Authorization", $"Bearer {accessToken}"); 
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        var response = await ExecuteAsync(request);

        if (response.StatusCode == HttpStatusCode.TooManyRequests ||
            response.StatusCode == HttpStatusCode.ServiceUnavailable)
        {
            const int scalingFactor = 2;
            var retryAfterMilliseconds = 1000;

            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(retryAfterMilliseconds);
                response = await ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                    break;

                retryAfterMilliseconds *= scalingFactor;
            }
        }

        if (!response.IsSuccessStatusCode)
            throw ConfigureErrorException(response);

        return response;
    }
    
    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithErrorHandling(request);
        return JsonConvert.DeserializeObject<T>(response.Content, JsonSettings);
    }

    protected override Exception ConfigureErrorException(RestResponse response) 
    {
        if (string.IsNullOrWhiteSpace(response.Content))
            return new(response.StatusCode.ToString());
        
        if (!response.Content.IsJson())
            return new(response.Content);
        
        var error = JsonConvert.DeserializeObject<ErrorDto>(response.Content, JsonSettings);

        if (error != null)
            return new(error.Message);

        return new(response.Content);
    }

    private string GetAccessToken(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var clientId = authenticationCredentialsProviders.Get(CredNames.ClientId).Value;
        var clientSecret = authenticationCredentialsProviders.Get(CredNames.ClientSecret).Value;

        var request = new RestRequest("/connect/token", Method.Post);
        request.AddParameter("grant_type", "client_credentials");
        request.AddParameter("client_id", clientId);
        request.AddParameter("client_secret", clientSecret);

        var response = new RestClient("https://login.lionbridge.com").Execute(request);
        
        if (!response.IsSuccessful)
            throw new("Failed to authorize. Please check the validity of your client ID and secret.");

        var accessToken = JsonConvert.DeserializeObject<AccessTokenDto>(response.Content, JsonSettings).AccessToken;
        return accessToken;
    }
}
