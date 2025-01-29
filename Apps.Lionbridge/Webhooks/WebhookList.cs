using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Webhooks.Handlers;
using Apps.Lionbridge.Webhooks.Inputs;
using Apps.Lionbridge.Webhooks.Payload;
using Apps.Lionbridge.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Linq;

namespace Apps.Lionbridge.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : LionbridgeInvocable(invocationContext)
{
    #region Job Webhooks

    [Webhook("On job status updated", typeof(JobStatusUpdatedHandler),
        Description = "Check for updates on jobs, as you are directly informed when the job is finished or cancelled")]
    public async Task<WebhookResponse<JobStatusUpdatedResponse>> OnJobStatusUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobStatusUpdatedInput input)
    {
        var data = JsonConvert.DeserializeObject<JobStatusUpdatedPayload>(webhookRequest.Body.ToString());
        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        var preflightResponse = new WebhookResponse<JobStatusUpdatedResponse>
        {
            HttpResponseMessage = null,
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };

        if (!string.IsNullOrEmpty(input.JobId) && input.JobId != data.JobId)
        {
            return preflightResponse;
        }

        bool archived = input.Archived ?? false;
        if (archived != data.Archived)
        {
            return preflightResponse;
        }

        bool deleted = input.Deleted ?? false;
        if (deleted != data.Deleted)
        {
            return preflightResponse;
        }
        
        if (input.StatusCodes != null && !input.StatusCodes.Contains(data.StatusCode))
        {
            return preflightResponse;
        }

        var jobDto = await GetJobDto(data.JobId);

        if (!String.IsNullOrEmpty(input.JobName) && !jobDto.JobName.ToLower().Contains(input.JobName.ToLower()))
        {
            return preflightResponse;
        }

        return new WebhookResponse<JobStatusUpdatedResponse>
        {
            HttpResponseMessage = null,
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = new JobStatusUpdatedResponse
            {
                Job = jobDto,
                Deleted = data.Deleted,
                Archived = data.Archived
            }
        };
    }

    #endregion

    #region Request Webhooks

    [Webhook("On request status updated", typeof(RequestStatusUpdatedHandler),
        Description =
            "Check for updates on requests, as you are directly informed when the request is finished or cancelled")]
    public async Task<WebhookResponse<RequestStatusUpdatedResponse>> OnRequestStatusUpdated(
        WebhookRequest webhookRequest, [WebhookParameter] GetRequestsInput requests)
    {
        var data = JsonConvert.DeserializeObject<RequestStatusUpdatedPayload>(webhookRequest.Body.ToString());
        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        var preflightResponse = new WebhookResponse<RequestStatusUpdatedResponse>
        {
            HttpResponseMessage = null,
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };
        
        if (requests.StatusCodes != null && !requests.StatusCodes.Any(x => data.StatusCode == x))
        {
            return preflightResponse;
        }

        if (!string.IsNullOrEmpty(requests.JobId) && requests.JobId != data.JobId)
        {
            return preflightResponse;
        }

        if (requests.RequestIds != null && !requests.RequestIds.Any(x => data.RequestIds.Contains(x)))
        {
            return preflightResponse;
        }

        var jobDto = await GetJobDto(data.JobId);
        if (!String.IsNullOrEmpty(requests.JobName) && !jobDto.JobName.ToLower().Contains(requests.JobName.ToLower()))
        {
            return preflightResponse;
        }

        var requestDtos = requests.RequestIds is null ? 
            await GetRequestsDto(data.JobId, data.RequestIds) : 
            await GetRequestsDto(data.JobId, data.RequestIds.Where(x => requests.RequestIds.Contains(x)));


        if (requests.StatusCodes != null)
        { requestDtos = requestDtos.Where(x => requests.StatusCodes.Contains(x.StatusCode)).ToList(); }
        
        return new WebhookResponse<RequestStatusUpdatedResponse>
        {
            HttpResponseMessage = null,
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = new RequestStatusUpdatedResponse
            {
                Job = jobDto,
                Requests = requestDtos,
            }
        };
    }

    [Webhook("On all requests in review", typeof(AllRequestsInReviewHandler),
        Description ="Given the job, this event is triggered when all requests of this jobs are in review and they can be downloaded")]
    public async Task<WebhookResponse<RequestStatusUpdatedResponse>> OnAllRequestsCompleted(
        WebhookRequest webhookRequest, [WebhookParameter] CompletedRequestsInput input)
    {
        var data = JsonConvert.DeserializeObject<RequestStatusUpdatedPayload>(webhookRequest.Body.ToString());
        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        var preflightResponse = new WebhookResponse<RequestStatusUpdatedResponse>
        {
            HttpResponseMessage = null,
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };
       
        if (input.JobId != data.JobId)
        {
            return preflightResponse;
        }

        var jobDto = await GetJobDto(data.JobId);

        var requestDtos = await GetRequestsDto(data.JobId, null);
        var allCompleted = requestDtos.All(x => x.StatusCode == "REVIEW_TRANSLATION");

        if (!allCompleted)
        {
            return preflightResponse;
        }

        return new WebhookResponse<RequestStatusUpdatedResponse>
        {
            HttpResponseMessage = null,
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = new RequestStatusUpdatedResponse
            {
                Job = jobDto,
                Requests = requestDtos,
            }
        };
    }

    #endregion

    private async Task<JobDto> GetJobDto(string jobId)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}");
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }

    private async Task<IEnumerable<RequestDto>> GetRequestsDto(string jobId, IEnumerable<string>? requestIds)
    {
        var requests = await GetRequests(jobId, requestIds);   
        return requests.Requests;
    }
}