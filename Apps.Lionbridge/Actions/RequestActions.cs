using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Responses.Request;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class RequestActions(InvocationContext invocationContext) : LionbridgeInvocable(invocationContext)
{
    [Action("Create requests", Description = "Create a new translation requests.")]
    public async Task<GetRequestsResponse> CreateRequest([ActionParameter] AddRequestsModel request, [ActionParameter] GetJobRequest jobRequest)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Add, Method.Post)
            .WithJsonBody(new
            {
                requestName = request.RequestName,
                sourceNativeId = request.SourceNativeId,
                sourceNativeLanguageCode = request.SourceNativeLanguageCode,
                targetNativeIds = request.TargetNativeIds,
                targetNativeLanguageCodes = request.TargetNativeLanguageCodes,
                wordCount = request.WordCount,
                fieldNames = request.FieldNames,
                fieldValues = request.FieldValues,
                fieldComments = request.FieldComments
            });

        var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(apiRequest);
        return new GetRequestsResponse { Requests = response.Embedded.Requests.ToList() };
    }
    
    [Action("Create request", Description = "Create a new translation request.")]
    public async Task<RequestDto> CreateSingleRequest([ActionParameter] AddRequestModel request, [ActionParameter] GetJobRequest jobRequest)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Add, Method.Post)
            .WithJsonBody(new
            {
                requestName = request.RequestName,
                sourceNativeId = request.SourceNativeId,
                sourceNativeLanguageCode = request.SourceNativeLanguageCode,
                targetNativeIds = request.TargetNativeIds,
                targetNativeLanguageCodes = new List<string> { request.TargetNativeLanguage },
                wordCount = request.WordCount,
                fieldNames = request.FieldNames,
                fieldValues = request.FieldValues,
                fieldComments = request.FieldComments
            });

        var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(apiRequest);
        return response.Embedded.Requests.First();
    }
    
    [Action("Get request", Description = "Get a translation request.")]
    public async Task<RequestDto> GetRequest([ActionParameter] GetRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" + $"{ApiEndpoints.Requests}/{request.RequestId}");
        return await Client.ExecuteWithErrorHandling<RequestDto>(apiRequest);
    }

    [Action("Delete request", Description = "Delete a translation request.")]
    public async Task DeleteRequest([ActionParameter] GetRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" + $"{ApiEndpoints.Requests}/{request.RequestId}", Method.Delete);
        await Client.ExecuteWithErrorHandling(apiRequest);
    }
    
    [Action("Approve request", Description = "Approve a translation request")]
    public async Task ApproveRequest([ActionParameter] GetRequests request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Approve, Method.Put)
            .WithJsonBody(new
            {
                requestIds = request.RequestIds
            });
        
        await Client.ExecuteWithErrorHandling(apiRequest);
    }
    
    [Action("Reject request", Description = "Reject a translation request")]
    public async Task RejectRequest([ActionParameter] GetRequests request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Reject, Method.Put)
            .WithJsonBody(new
            {
                requestIds = request.RequestIds
            });
        
        await Client.ExecuteWithErrorHandling(apiRequest);
    }
    
    [Action("Update request content", Description = "Update a translation request content")]
    public async Task<GetRequestsResponse> UpdateRequestContent([ActionParameter]GetRequests request, [ActionParameter] UpdateContentRequest updateRequestContentModel)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" + ApiEndpoints.Requests, Method.Put)
            .WithJsonBody(new
            {
                requestIds = request.RequestIds,
                fieldNames = updateRequestContentModel.FieldNames,
                fieldValues = updateRequestContentModel.FieldValues,
                fieldComments = updateRequestContentModel.FieldComments
            });
        
        var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(apiRequest);
        return new GetRequestsResponse { Requests = response.Embedded.Requests.ToList() };
    }
}