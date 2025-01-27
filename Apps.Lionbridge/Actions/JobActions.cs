using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Provider;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Microsoft.VisualBasic;
using RestSharp;
using System.Text.Json;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class JobActions(InvocationContext invocationContext) : LionbridgeInvocable(invocationContext)
{
    [Action("Create job", Description = "Create a new job")]
    public async Task<JobDto> CreateJob([ActionParameter] CreateJobRequest input)
    {
        var request = new LionbridgeRequest("/jobs", Method.Post)
            .WithJsonBody(new
            {
                jobName = input.JobName,
                description = input.Description,
                providerId = input.ProviderId,
                extendedMetadata = EnumerableExtensions.ToDictionary(input.MetadataKeys, input.MetadataValues),
                labels = EnumerableExtensions.ToDictionary(input.LabelKeys, input.LabelValues),
                dueDate = input.dueDate.HasValue ? input.dueDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : null
            });

        return await Client.ExecuteWithErrorHandling<JobDto>(request);
    }

    [Action("Delete job", Description = "Delete a job")]
    public async Task DeleteJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}", Method.Delete);
        await Client.ExecuteWithErrorHandling(apiRequest);
    }

    [Action("Get job", Description = "Get a job")]
    public async Task<JobDto> GetJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}");
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }

    [Action("Update job",
        Description =
            "Update a job, update only the fields that are specified. To complete a job, set the Job status to 'Completed'. To set a job to 'In translation', set the Job status to 'In translation'")]
    public async Task<JobDto> UpdateJob([ActionParameter] GetJobRequest jobRequest,
        [ActionParameter] UpdateJobRequest request)
    {
        var changed = HasJobChanged(request);

        JobDto? job = null;
        if (changed)
        {
            var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}", Method.Patch)
                .WithJsonBody(new
                {
                    jobName = request.JobName,
                    description = request.Description,
                    providerId = request.ProviderId,
                    extendedMetadata = EnumerableExtensions.ToDictionary(request.MetadataKeys, request.MetadataValues),
                    labels = EnumerableExtensions.ToDictionary(request.LabelKeys, request.LabelValues),
                    dueDate = request.DueDate,
                    shouldQuote = request.ShouldQuote,
                    connectorName = request.ConnectorName,
                    connectorVersion = request.ConnectorVersion,
                    serviceType = request.ServiceType
                });
            
            job = await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
        }

        //if (request.JobCompletionStatus != null)
        //{
        //    job = await UpdateJobCompletionStatus(jobRequest.JobId, request.JobCompletionStatus, job);
        //}
        return job ?? await GetJob(jobRequest);
    }

    [Action("Submit job", Description = "Send a job for translation with a selected provider")]
    public async Task<JobDto> SubmitJob([ActionParameter] GetJobRequest request,
        [ActionParameter] GetProviderRequest providerRequest)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/submit", Method.Put)
            .WithJsonBody(new { providerId = providerRequest.ProviderId });

        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }

    [Action("Get job extended metadata", Description = "Get extended metadata value for a given key")]
    public async Task<string> GetJobMetadata([ActionParameter] GetJobRequest request, [ActionParameter] string Key )
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}");
        var response = await Client.ExecuteWithErrorHandling(apiRequest);
        using JsonDocument doc = JsonDocument.Parse(response.Content);
        JsonElement root = doc.RootElement;
        if (root.TryGetProperty("extendedMetadata", out JsonElement extendedMetadata) &&
            extendedMetadata.ValueKind == JsonValueKind.Object)
        {
            var metadata = new Dictionary<string, string>();
            Dictionary<string, string> metadataDict = JsonSerializer.Deserialize<Dictionary<string, string>>(extendedMetadata.GetRawText());
            if (metadataDict.TryGetValue(Key, out string specificValue))
            {
                return specificValue;
            }
            else
            {
                throw new PluginMisconfigurationException("The specified key was not found within the extended metadata");
            }

        }

        throw new PluginMisconfigurationException("No extended metata was found for Job ID "+ request.JobId);
    }

    [Action("Archive job", Description = "Move a job to storage for safekeeping")]
    public async Task<JobDto> ArchiveJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/archive", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }

    [Action("Unarchive job", Description = "Retrieve a job from storage back into active status")]
    public async Task<JobDto> UnarchiveJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/unarchive", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }

    private async Task<JobDto> CompleteJob(string jobId)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}/complete", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }

    private async Task<JobDto> IntranslateJob(string jobId)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}/intranslation", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    private bool HasJobChanged(UpdateJobRequest request)
    {
        return request.JobName != null || request.Description != null || request.ProviderId != null ||
               (request.MetadataKeys != null && request.MetadataValues != null) ||
               (request.LabelKeys != null && request.LabelValues != null) || request.DueDate != null ||
               request.ShouldQuote != null || request.ConnectorName != null ||
               request.ConnectorVersion != null || request.ServiceType != null;
    }
    
    private async Task<JobDto?> UpdateJobCompletionStatus(string jobId, string status, JobDto? currentJob)
    {
        switch (status)
        {
            case "COMPLETED":
                return await CompleteJob(jobId);
            case "IN_TRANSLATION":
                return await IntranslateJob(jobId);
            default:
                return currentJob;
        }
    }
}