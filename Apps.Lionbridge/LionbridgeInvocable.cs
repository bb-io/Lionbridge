using Apps.Lionbridge.Api;
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
}