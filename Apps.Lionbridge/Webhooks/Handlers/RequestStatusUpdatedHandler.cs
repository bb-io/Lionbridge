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

public class RequestStatusUpdatedHandler : BaseWebhookHandler, IAfterSubscriptionWebhookEventHandler<RequestStatusUpdatedResponse>
{
    const string SubscriptionEvent = "REQUEST_STATUS_UPDATED";
    GetRequestsInput input;

    public RequestStatusUpdatedHandler(InvocationContext invocationContext, [WebhookParameter] GetRequestsInput requests)
        : base(invocationContext, SubscriptionEvent)
    {
        input = requests;
    }
    protected override string[] GetStatusCodes()
    {
        return new[] { "IN_TRANSLATION", "REVIEW_TRANSLATION", "CANCELLED" };
    }

    public async Task<AfterSubscriptionEventResponse<RequestStatusUpdatedResponse>> OnWebhookSubscribedAsync()
    {
        if (input.RequestIds != null && input.RequestIds.Any() && input.StatusCodes != null && input.StatusCodes.Any() && input.JobId != null)
        {
            
            RestRequest apiRequest = new LionbridgeRequest(
        $"{ApiEndpoints.Jobs}/{input.JobId}" + ApiEndpoints.Requests,
        Method.Get);

            var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(apiRequest);
            var requests = response.Embedded.Requests.ToList();
            
            requests = requests.Where(x => input.RequestIds.Contains(x.RequestId))?.ToList();
            requests = requests?.Where(x => input.StatusCodes.Contains(x.StatusCode))?.ToList();
            return new AfterSubscriptionEventResponse<RequestStatusUpdatedResponse>()
            {
                Result = new RequestStatusUpdatedResponse
                {
                    Requests = requests ?? null!
                }
            };
        }

        return null!;
    }
}