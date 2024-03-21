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
                labels = EnumerableExtensions.ToDictionary(input.LabelKeys, input.LabelValues)
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
    
    [Action("Update job", Description = "Update a job, update only the fields that are specified. To complete a job, set the Job status to 'Completed'. To set a job to 'In translation', set the Job status to 'In translation'")]
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
            .WithJsonBody(new
            {
                jobName = apiUpdateRequest.JobName,
                description = apiUpdateRequest.Description,
                providerId = apiUpdateRequest.ProviderId,
                extendedMetadata = apiUpdateRequest.ExtendedMetadata,
                labels = apiUpdateRequest.Labels,
                dueDate = apiUpdateRequest.DueDate,
                shouldQuote = apiUpdateRequest.ShouldQuote,
                connectorName = apiUpdateRequest.ConnectorName,
                connectorVersion = apiUpdateRequest.ConnectorVersion,
                serviceType = apiUpdateRequest.ServiceType
            });
        
        if(request.JobCompletionStatus != null)
        {
            if(request.JobCompletionStatus == "COMPLETED")
            {
                return await CompleteJob(jobRequest.JobId);
            }
            
            if(request.JobCompletionStatus == "IN_TRANSLATION")
            {
                return await IntranslateJob(jobRequest.JobId);
            }
        }

        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    [Action("Submit job", Description = "Send a job for translation with a selected provider")]
    public async Task<JobDto> SubmitJob([ActionParameter] GetJobRequest request, [ActionParameter] GetProviderRequest providerRequest)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}/submit", Method.Put)
            .WithJsonBody(new { providerId = providerRequest.ProviderId });
        
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
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
    
    protected async Task<JobDto> CompleteJob(string jobId)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}/complete", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
    
    protected async Task<JobDto> IntranslateJob(string jobId)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}/intranslation", Method.Put);
        return await Client.ExecuteWithErrorHandling<JobDto>(apiRequest);
    }
}