using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Webhooks.Inputs;
using Apps.Lionbridge.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.Lionbridge.Webhooks.Handlers;

public class JobStatusUpdatedHandler : BaseWebhookHandler, IAfterSubscriptionWebhookEventHandler<JobStatusUpdatedResponse>
    
{
    const string SubscriptionEvent = "JOB_STATUS_UPDATED";
    JobStatusUpdatedInput input;

    public JobStatusUpdatedHandler(InvocationContext invocationContext, [WebhookParameter] JobStatusUpdatedInput request) 
        : base(invocationContext, SubscriptionEvent) 
    {
        input = request;
    }
    
    protected override string[] GetStatusCodes()
    {
        return ["SENT_TO_PROVIDER", "IN_TRANSLATION", "CANCELLED"];
    }

    public async Task<AfterSubscriptionEventResponse<JobStatusUpdatedResponse>> OnWebhookSubscribedAsync()
    {
        if ( input.JobId != null && input.StatusCodes != null && input.StatusCodes.Any())
        {
            var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{input.JobId}");
            var job = await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);

            if (input.StatusCodes.Contains(job.StatusCode))
            {
                return new AfterSubscriptionEventResponse<JobStatusUpdatedResponse>()
                {
                    Result = new JobStatusUpdatedResponse 
                    {
                        Job = job,
                        Archived = job.Archived
                    }
                };
            }
        }

        return null!;
    }
}