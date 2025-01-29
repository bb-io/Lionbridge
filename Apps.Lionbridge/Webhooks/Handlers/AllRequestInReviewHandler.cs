using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Responses.Request;
using Apps.Lionbridge.Webhooks.Inputs;
using Apps.Lionbridge.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;
using System.Net;

namespace Apps.Lionbridge.Webhooks.Handlers;

public class AllRequestsInReviewHandler : BaseWebhookHandler, IAfterSubscriptionWebhookEventHandler<RequestStatusUpdatedResponse>
{
    const string SubscriptionEvent = "REQUEST_STATUS_UPDATED";
    CompletedRequestsInput input;

    public AllRequestsInReviewHandler(InvocationContext invocationContext, [WebhookParameter] CompletedRequestsInput input)
        : base(invocationContext, SubscriptionEvent)
    {
        input = input;
    }
    protected override string[] GetStatusCodes()
    {
        return new[] { "IN_TRANSLATION", "REVIEW_TRANSLATION", "CANCELLED" };
    }

    public async Task<AfterSubscriptionEventResponse<RequestStatusUpdatedResponse>> OnWebhookSubscribedAsync()
    {
        RestRequest apiRequest = new LionbridgeRequest(
            $"{ApiEndpoints.Jobs}/{input.JobId}" + ApiEndpoints.Requests,
            Method.Get);

        var response = await Client.Paginate<RequestsWrapper>(apiRequest);
        var requests = response.SelectMany(x => x.Requests);

        if (requests.All(x => x.StatusCode == "REVIEW_TRANSLATION"))
        {
            var jobRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{input.JobId}");
            var job = await Client.ExecuteWithErrorHandling<JobDto>(jobRequest);

            return new AfterSubscriptionEventResponse<RequestStatusUpdatedResponse>()
            {
                Result = new RequestStatusUpdatedResponse
                {
                    Requests = requests ?? null!,
                    Job = job,
                }
            };
        }

        return null;
    }
}