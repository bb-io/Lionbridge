using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Responses.Request;
using Apps.Lionbridge.Models.Responses.TranslationContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class RequestActions(InvocationContext invocationContext) : LionbridgeInvocable(invocationContext)
{
    [Action("Get requests", Description = "Get translation requests.")]
    public async Task<GetRequestsResponse> GetRequests([ActionParameter] GetRequestsAsOptional jobRequest)
    {
        RestRequest apiRequest = new LionbridgeRequest(
            $"{ApiEndpoints.Jobs}/{jobRequest.JobId}" + ApiEndpoints.Requests,
            Method.Get);

        var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(apiRequest);
        var requests = response.Embedded.Requests.ToList();
        if (jobRequest.RequestIds != null && jobRequest.RequestIds.Any())
        {
            requests = requests.Where(x => jobRequest.RequestIds.Contains(x.RequestId)).ToList();
        }

        return new GetRequestsResponse { Requests = requests };
    }

    [Action("Create source content request", Description = "Create a new translation request.")]
    public async Task<RequestDto> CreateSingleRequest([ActionParameter] AddSourceRequestModel request,
        [ActionParameter] GetJobRequest jobRequest)
    {
        string sourceContentId = await CreateTranslationContent(jobRequest.JobId, request.FieldsKeys, request.FieldsValues);
        
        var metadata = EnumerableExtensions.ToDictionary(request.MetadataKeys, request.MetadataValues);
        var apiRequest =
            new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Add,
                    Method.Post)
                .WithJsonBody(new
                {
                    requestName = request.RequestName,
                    sourceNativeId = request.SourceNativeId,
                    sourceNativeLanguageCode = request.SourceNativeLanguageCode,
                    targetNativeIds = request.TargetNativeIds,
                    targetNativeLanguageCodes = new List<string> { request.TargetNativeLanguage },
                    wordCount = request.WordCount,
                    extendedMetadata = metadata,
                    sourcecontentId = sourceContentId
                });

        var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(apiRequest);
        return response.Embedded.Requests.First();
    }

    [Action("Get request", Description = "Get a translation request.")]
    public async Task<RequestDto> GetRequest([ActionParameter] GetRequest request)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" +
                                               $"{ApiEndpoints.Requests}/{request.RequestId}");
        return await Client.ExecuteWithErrorHandling<RequestDto>(apiRequest);
    }

    [Action("Delete request", Description = "Delete a translation request.")]
    public async Task<RequestDto> DeleteRequest([ActionParameter] GetRequest request)
    {
        var apiRequest =
            new LionbridgeRequest(
                $"{ApiEndpoints.Jobs}/{request.JobId}" + $"{ApiEndpoints.Requests}/{request.RequestId}", Method.Delete);

        return await Client.ExecuteWithErrorHandling<RequestDto>(apiRequest);
    }

    [Action("Approve request", Description = "Approve a translation request")]
    public async Task ApproveRequest([ActionParameter] GetRequests request)
    {
        var apiRequest =
            new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Approve,
                    Method.Put)
                .WithJsonBody(new
                {
                    requestIds = request.RequestIds
                });

        await Client.ExecuteWithErrorHandling(apiRequest);
    }

    [Action("Reject request", Description = "Reject a translation request")]
    public async Task RejectRequest([ActionParameter] GetRequests request)
    {
        var apiRequest =
            new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Reject,
                    Method.Put)
                .WithJsonBody(new
                {
                    requestIds = request.RequestIds
                });

        await Client.ExecuteWithErrorHandling(apiRequest);
    }

    [Action("Update request content", Description = "Update a translation request content")]
    public async Task<GetRequestsResponse> UpdateRequestContent([ActionParameter] GetRequests request,
        [ActionParameter] UpdateContentRequest updateRequestContentModel)
    {
        throw new NotImplementedException("This action is not implemented yet.");

        var apiRequest =
            new LionbridgeRequest(
                    $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.Requests}/{request}" + ApiEndpoints.Requests +
                    ApiEndpoints.UpdateContent,
                    Method.Patch)
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

    private async Task<string> CreateTranslationContent(string jobId, IEnumerable<string> keys, IEnumerable<string> values)
    {
        var dictionary = EnumerableExtensions.ToDictionary(keys, values);
        var listOfKeyValuePairs = dictionary.Select(x => new FieldDto { Key = x.Key, Value = x.Value })
            .ToList();
        
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}{ApiEndpoints.SourceContent}", Method.Post)
            .WithJsonBody(new
            {
                fields = listOfKeyValuePairs
            });
        
        var response = await Client.ExecuteWithErrorHandling<TranslationContentResponse>(apiRequest);
        return response.SourceContentId;
    }
}