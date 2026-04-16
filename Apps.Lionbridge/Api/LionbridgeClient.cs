using System.Net;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Responses.Request;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using Polly;
using RestSharp;

namespace Apps.Lionbridge.Api;

public class LionbridgeClient : BlackBirdRestClient
{
    private static readonly TimeSpan PaginationDelay = TimeSpan.FromMilliseconds(200);
    private readonly ResiliencePipeline<RestResponse> _retryPolicy;

    protected override JsonSerializerSettings JsonSettings =>
        new() { MissingMemberHandling = MissingMemberHandling.Ignore };

    public LionbridgeClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        : base(new RestClientOptions
            { ThrowOnAnyError = false, BaseUrl = authenticationCredentialsProviders.First(c => c.KeyName == CredNames.BaseUrl).Value.ToUri() })
    {
        var accessToken = GetAccessToken(authenticationCredentialsProviders);
        this.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
        _retryPolicy = LionbridgePollyPolicies.GetRetryPolicy();
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        var response = await ExecuteSafeAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var content = response.Content ?? string.Empty;

            if (response.ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) == true ||
                content.TrimStart().StartsWith("<"))
            {
                var title = ExtractHtmlTagContent(content, "title");
                var body = ExtractHtmlTagContent(content, "body");
                var message = $"{title}: \nError Description: {body}";

                if (title.ToLower().Contains("sign in") || title.ToLower().Contains("log in"))
                {
                    throw new PluginApplicationException("Failed to authenticate. Please check your account permissions and try again");
                }

                throw new PluginApplicationException(message);
            }
            else
            {
                throw ConfigureErrorException(response);
            }
        }
        return response;
    }
    
    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithErrorHandling(request);
        return JsonConvert.DeserializeObject<T>(response.Content, JsonSettings);
    }

    public async Task<IEnumerable<T>> Paginate<T>(RestRequest request)
    {
        var result = new List<T>();

        while (true)
        {
            var response = await ExecuteWithErrorHandling<EmbeddedItemsWrapper<T>>(request); 
            if (response.Embedded == null) 
                break;

            result.Add(response.Embedded);

            var href = response?.Links?.Next?.Href;

            if (string.IsNullOrWhiteSpace(href))
                break;

            if (href.StartsWith("/v2/", StringComparison.OrdinalIgnoreCase))
            {
                href = href.Substring(3);  // remove "/v2"
            }

            await Task.Delay(PaginationDelay);
            request = new RestRequest(href, Method.Get);
        }

        return result;
    }

    protected override Exception ConfigureErrorException(RestResponse response) 
    {
        try
        {
            if (string.IsNullOrWhiteSpace(response.Content))
                return new(response.StatusCode.ToString());
        
            if (!response.Content.IsJson())
                return new(response.Content);
        
            var error = JsonConvert.DeserializeObject<ErrorDto>(response.Content, JsonSettings);

            if (error != null)
                return new PluginApplicationException(error.Message);

            return new PluginApplicationException(response.Content);
        }
        catch (Exception e)
        {
            return new Exception($"Error was thrown while executing request, response content: {response.Content}; status code: {response.StatusCode}; Exception message: {e.Message}");
        }
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
            throw new PluginMisconfigurationException("Failed to authorize. Please check the validity of your client ID and secret.");

        var tokenDto = JsonConvert.DeserializeObject<AccessTokenDto>(response.Content, JsonSettings);
        return tokenDto?.AccessToken ?? throw new Exception("Access token is missing in GetAccessToken response");
    }

    private static string ExtractHtmlTagContent(string html, string tagName)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        var regex = new System.Text.RegularExpressions.Regex($"<{tagName}.*?>(.*?)</{tagName}>",
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        var match = regex.Match(html);
        return match.Success ? match.Groups[1].Value.Trim() : "N/A";
    }

    private async Task<RestResponse> ExecuteSafeAsync(RestRequest request)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(_ => new ValueTask<RestResponse>(ExecuteAsync(request)),
                CancellationToken.None);
        }
        catch (Exception ex) when (ex is not PluginApplicationException)
        {
            throw new PluginApplicationException($"Request failed: {ex.Message}", ex);
        }
    }
}
