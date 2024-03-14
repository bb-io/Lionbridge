using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Provider;
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
    
    [Action("Update job", Description = "Update a translation job")]
    public async Task<JobDto> UpdateJob([ActionParameter] GetJobRequest jobRequest, [ActionParameter] UpdateJobRequest request)
    {
        var apiUpdateRequest = new UpdateJobApiRequest();
        
        if(request.JobName != null)
        {
            apiUpdateRequest.JobName = request.JobName;
        }
        
        if(request.Description != null)
        {
            apiUpdateRequest.Description = request.Description;
        }
        
        if(request.ProviderId != null)
        {
            apiUpdateRequest.ProviderId = request.ProviderId;
        }
        
        if(request.MetadataKeys != null && request.MetadataValues != null)
        {
            apiUpdateRequest.ExtendedMetadata = EnumerableExtensions.ToDictionary(request.MetadataKeys, request.MetadataValues);
        }
        
        if(request.LabelKeys != null && request.LabelValues != null)
        {
            apiUpdateRequest.Labels = EnumerableExtensions.ToDictionary(request.LabelKeys, request.LabelValues);
        }
        
        if(request.DueDate != null)
        {
            apiUpdateRequest.DueDate = request.DueDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        
        if(request.CustomData != null)
        {
            apiUpdateRequest.CustomData = request.CustomData;
        }
        
        if(request.ShouldQuote != null)
        {
            apiUpdateRequest.ShouldQuote = request.ShouldQuote;
        }
        
        if(request.ConnectorName != null)
        {
            apiUpdateRequest.ConnectorName = request.ConnectorName;
        }
        
        if(request.ConnectorVersion != null)
        {
            apiUpdateRequest.ConnectorVersion = request.ConnectorVersion;
        }
        
        if(request.ServiceType != null)
        {
            apiUpdateRequest.ServiceType = request.ServiceType;
        }
        
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}", Method.Patch)
            .WithJsonBody(apiUpdateRequest);

        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    [Action("Submit job", Description = "Submit a translation job")]
    public async Task<JobDto> SubmitJob([ActionParameter] GetJobRequest request, [ActionParameter] GetProviderRequest providerRequest)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/submit", Method.Put)
            .WithJsonBody(new { providerId = providerRequest.ProviderId });
        
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    [Action("Archive job", Description = "Archive a translation job")]
    public async Task<JobDto> ArchiveJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/archive", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    [Action("Unarchive job", Description = "Unarchive a translation job")]
    public async Task<JobDto> UnarchiveJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/unarchive", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    [Action("Complete job", Description = "Complete a translation job")]
    public async Task<JobDto> CompleteJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/complete", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    [Action("Intranslate job", Description = "Set job status to IN_TRANSLATION. Allows further translations from being imported again. Only valid when job is currently COMPLETED")]
    public async Task<JobDto> IntranslateJob([ActionParameter] GetJobRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/intranslation", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
}