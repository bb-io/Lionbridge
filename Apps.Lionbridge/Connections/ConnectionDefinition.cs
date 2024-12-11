using Apps.Lionbridge.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Lionbridge.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>()
    {
        new()
        {
            Name = "Lionbridge connection",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionProperties = new List<ConnectionProperty>
            {
                new(CredNames.ClientId) { DisplayName = "Client ID" },
                new(CredNames.ClientSecret) { DisplayName = "Client secret", Sensitive = true },
                new(CredNames.BaseUrl)
                {
                    DisplayName="Base Url",
                    Description = "Select the base URL for Lionbridge API",
                    DataItems= [new("https://content-api.staging.lionbridge.com/v2", "Staging environment"),
                                new("https://contentapi.lionbridge.com/v2","Production environment")]
                }
            }
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
    {
        var clientId = values.First(v => v.Key == CredNames.ClientId);
        yield return new AuthenticationCredentialsProvider(clientId.Key, clientId.Value);

        var clientSecret = values.First(v => v.Key == CredNames.ClientSecret);
        yield return new AuthenticationCredentialsProvider(clientSecret.Key, clientSecret.Value);

        var baseUrl = values.First(v => v.Key == CredNames.BaseUrl);
        yield return new AuthenticationCredentialsProvider(baseUrl.Key, baseUrl.Value);
    }
       
}