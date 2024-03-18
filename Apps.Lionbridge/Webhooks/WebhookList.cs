using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Webhooks.Inputs;
using Apps.Lionbridge.Webhooks.Payload;
using Apps.Lionbridge.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.Lionbridge.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : LionbridgeInvocable(invocationContext)
{
    #region Job Webhooks

    [Webhook("Job status updated",
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

        var jobDto = await GetJobDto(data);
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
    
    private async Task<JobDto> GetJobDto(JobStatusUpdatedPayload data)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{data.JobId}");
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }

    #endregion
}