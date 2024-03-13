using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.Job;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class JobActions(InvocationContext invocationContext) : LionbridgeInvocable(invocationContext)
{
    [Action("Create job", Description = "Create a new translation job.")]
    public async Task<JobDto> CreateJob([ActionParameter] CreateJobRequest input)
    {
        var request = new LionbridgeRequest("/jobs", Method.Post)
            .WithJsonBody(new
            {
                jobName = input.JobName,
                description = input.Description,
                providerId = input.ProviderId,
                extendedMetadata = EnumerableExtensions.ToDictionary(input.MetadataKeys, input.MetadataValues),
                labels = EnumerableExtensions.ToDictionary(input.LabelKeys, input.LabelValues)
            });

        return await Client.ExecuteWithErrorHandling<JobDto>(request);
    }

    [Action("Delete job", Description = "Delete a translation job.")]
    public async Task DeleteJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}", Method.Delete);
        await Client.ExecuteWithErrorHandling(apiRequest);
    }
    
    [Action("Get job", Description = "Get a translation job.")]
    public async Task<JobDto> GetJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}");
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
}