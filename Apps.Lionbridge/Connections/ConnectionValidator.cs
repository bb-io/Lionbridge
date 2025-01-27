using Apps.Lionbridge.Api;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Lionbridge.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        CancellationToken cancellationToken)
    {        
        try
        {
            var client = new LionbridgeClient(authenticationCredentialsProviders);
            var request = new LionbridgeRequest("/providers");
            await client.ExecuteWithErrorHandling(request);
            return new ConnectionValidationResponse
            {
                IsValid = true
            };
        }
        catch (Exception exception)
        {
            return new ConnectionValidationResponse
            {
                IsValid = false,
                Message = exception.Message
            };
        }
    }
}