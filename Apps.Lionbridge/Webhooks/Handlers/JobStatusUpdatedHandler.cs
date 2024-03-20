using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Lionbridge.Webhooks.Handlers;

public class JobStatusUpdatedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "JOB_STATUS_UPDATED";

    public JobStatusUpdatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    
    protected override string[] GetStatusCodes()
    {
        return ["IN_TRANSLATION", "CANCELLED", "SENT_TO_PROVIDER"];
    }
    
    protected override string GetEventType()
    {
        return "JOB_UPDATE";
    }
}