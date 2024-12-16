using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.Webhooks.Handlers;

public class RequestStatusUpdatedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "REQUEST_STATUS_UPDATED";

    public RequestStatusUpdatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    protected override string[] GetStatusCodes()
    {
        return new[] { "CREATED", "SENDING", "SENT_TO_PROVIDER", "IN_TRANSLATION", "REVIEW_TRANSLATION", "TRANSLATION_REJECTED", "TRANSLATION_APPROVED", "CANCELLED" };
    }
}