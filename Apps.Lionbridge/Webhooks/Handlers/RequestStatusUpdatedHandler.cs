using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.Webhooks.Handlers;

public class RequestStatusUpdatedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "REQUEST_STATUS_UPDATED";

    public RequestStatusUpdatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    protected override string[] GetStatusCodes()
    {
        return new[] { "IN_TRANSLATION", "CANCELLED", "REVIEW_TRANSLATION" };
    }

    protected override string GetEventType()
    {
        return "REQUEST_UPDATE";
    }
}