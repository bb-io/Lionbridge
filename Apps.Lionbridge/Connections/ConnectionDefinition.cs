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
            ConnectionUsage = ConnectionUsage.Actions,
            ConnectionProperties = new List<ConnectionProperty>
            {
                new(CredNames.ClientId) { DisplayName = "Client ID" },
                new(CredNames.ClientSecret) { DisplayName = "Client secret", Sensitive = true }
            }
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
        => values.Select(x =>
                new AuthenticationCredentialsProvider(AuthenticationCredentialsRequestLocation.None, x.Key, x.Value))
            .ToList();
}