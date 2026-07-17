using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Webhooks.Inputs;
using Apps.Lionbridge.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.Lionbridge.Polling;

[PollingEventList]
public class PollingList(InvocationContext invocationContext) : LionbridgeInvocable(invocationContext)
{
    [PollingEvent("On all requests in review [Polling]",
        Description = "Polling alternative to the 'On all requests in review' webhook trigger. Triggered when all requests in a job have reached the review translation status.")]
    public async Task<PollingEventResponse<RequestsInReviewMemory, RequestStatusUpdatedResponse>> OnAllRequestsInReview(
        PollingEventRequest<RequestsInReviewMemory> request,
        [PollingEventParameter] CompletedRequestsInput input)
    {
        var requestsResponse = await GetRequests(input.JobId, null);
        var requests = requestsResponse.Requests.ToList();

        bool allInReview = requests.Any() && requests.All(x => x.StatusCode == "REVIEW_TRANSLATION");
        bool wasAlreadyInReview = request.Memory?.AllInReview ?? false;
        var newMemory = new RequestsInReviewMemory { AllInReview = allInReview };

        if (!allInReview || wasAlreadyInReview)
        {
            return new PollingEventResponse<RequestsInReviewMemory, RequestStatusUpdatedResponse>
            {
                FlyBird = false,
                Memory = newMemory
            };
        }

        var jobRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{input.JobId}");
        var job = await Client.ExecuteWithErrorHandling<JobDto>(jobRequest);

        return new PollingEventResponse<RequestsInReviewMemory, RequestStatusUpdatedResponse>
        {
            FlyBird = true,
            Memory = newMemory,
            Result = new RequestStatusUpdatedResponse
            {
                Requests = requests,
                Job = job
            }
        };
    }
}
