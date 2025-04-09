using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests;
using Apps.Lionbridge.Models.Requests.File;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Responses.Request;
using Apps.Lionbridge.Models.Responses.TranslationContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Exceptions;
using System.Text.Json;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class RequestActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : LionbridgeInvocable(invocationContext)
{
    [Action("Search requests", Description = "Given a job, get all related requests and whether they are in review or not")]
    public async Task<GetRequestsResponse> GetRequests([ActionParameter] GetRequestsAsOptional jobRequest)
    {
        return await GetRequests(jobRequest.JobId, jobRequest.RequestIds);
    }

    // [Action("Create source content request", Description = "Create a new translation request.")]
    public async Task<GetRequestsResponse> CreateSingleRequest([ActionParameter] AddSourceRequestModel request,
        [ActionParameter] GetJobRequest jobRequest)
    {
        string sourceContentId =
            await CreateTranslationContent(jobRequest.JobId, request.FieldsKeys, request.FieldsValues);

        var metadata = EnumerableExtensions.ToDictionary(request.MetadataKeys, request.MetadataValues);
        var apiRequest =
            new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Add,
                    Method.Post)
                .WithJsonBody(new
                {
                    requestName = request.RequestName ?? Guid.NewGuid().ToString(),
                    sourceNativeId = request.SourceNativeId ?? Guid.NewGuid().ToString(),
                    sourceNativeLanguageCode = request.SourceNativeLanguageCode,
                    targetNativeIds = request.TargetNativeIds,
                    targetNativeLanguageCodes = new List<string> { request.TargetNativeLanguage },
                    wordCount = request.WordCount,
                    extendedMetadata = metadata,
                    sourcecontentId = sourceContentId
                });

        var response = await Client.Paginate<RequestsWrapper>(apiRequest);
        var requests = response.SelectMany(x => x.Requests);
        return new GetRequestsResponse() { Requests = requests ?? new List<RequestDto>() };
    }

    [Action("Create file requests", Description = "Start a new request to translate a document")]
    public async Task<GetRequestsResponse> CreateFileRequest([ActionParameter] GetJobRequest jobRequest,
        [ActionParameter] AddSourceFileRequest sourceFileRequest)
    {
        if (sourceFileRequest.TargetNativeLanguage.Count() > 100)
        {
            throw new PluginMisconfigurationException("You can only request a max of 100 target languages. Please reduce the amount of target languages.");
        }

        string originalFileName = sourceFileRequest.FileName ?? sourceFileRequest.File.Name;
        string encodedFileName = Uri.EscapeDataString(originalFileName);
        sourceFileRequest.FileName = encodedFileName;

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(encodedFileName);

        var uploadResponse = await UploadFmsFile(jobRequest.JobId, new AddFileRequest(sourceFileRequest), fileManagementClient);

        var metadata =
            EnumerableExtensions.ToDictionary(sourceFileRequest.MetadataKeys, sourceFileRequest.MetadataValues);
        var apiRequest =
            new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Add,
                    Method.Post)
                .WithJsonBody(new
                {
                    requestName = sourceFileRequest.RequestName ?? fileNameWithoutExtension,
                    sourceNativeId = sourceFileRequest.SourceNativeId ?? Guid.NewGuid().ToString(),
                   // sourcecontentId = sourceFileRequest.SourceContentId,
                    sourceNativeLanguageCode = sourceFileRequest.SourceNativeLanguageCode,
                    targetNativeIds = sourceFileRequest.TargetNativeIds,
                    targetNativeLanguageCodes = sourceFileRequest.TargetNativeLanguage.ToArray(),
                    wordCount = sourceFileRequest.WordCount,
                    extendedMetadata = metadata,
                    fmsFileId = uploadResponse.FmsFileId
                });

        var response = await Client.Paginate<RequestsWrapper>(apiRequest);
        var requests = response.SelectMany(x => x.Requests);
        return new GetRequestsResponse() { Requests = requests ?? new List<RequestDto>()};
    }

    [Action("Get request", Description = "View details of a specific translation request")]
    public async Task<RequestDto> GetRequest([ActionParameter] GetRequest request)
    {
        return await GetRequest(request.JobId, request.RequestId);
    }

    [Action("Delete request", Description = "Remove a translation request no longer needed")]
    public async Task<RequestDto> DeleteRequest([ActionParameter] GetRequest request)
    {
        var apiRequest =
            new LionbridgeRequest(
                $"{ApiEndpoints.Jobs}/{request.JobId}" + $"{ApiEndpoints.Requests}/{request.RequestId}", Method.Delete);

        return await Client.ExecuteWithErrorHandling<RequestDto>(apiRequest);
    }

    [Action("Approve requests", Description = "Approve translated content for specific requests")]
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

    [Action("Reject requests", Description = "Reject translated content for specific requests")]
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

    [Action("Update request details", Description = "Make changes to the details of an existing translation request.")]
    public async Task<RequestDto> UpdateRequestContent([ActionParameter] GetRequest request,
        [ActionParameter] UpdateRequestModel updateRequestContentModel)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.Requests}/{request}";
        var apiRequest =
            new LionbridgeRequest(endpoint, Method.Patch)
                .WithJsonBody(new
                {
                    requestName = updateRequestContentModel.RequestName,
                    sourceNativeId = updateRequestContentModel.SourceNativeId,
                    sourceNativeLanguageCode = updateRequestContentModel.SourceNativeLanguageCode,
                    targetNativeId = updateRequestContentModel.TargetNativeId,
                    targetNativeLanguageCode = updateRequestContentModel.TargetNativeLanguageCode,
                    extendedMetadata = EnumerableExtensions.ToDictionary(updateRequestContentModel.MetadataKeys,
                        updateRequestContentModel.MetadataValues),
                    fileId = updateRequestContentModel.FileId,
                    sourceContentId = updateRequestContentModel.SourceContentId
                });

        var response = await Client.ExecuteWithErrorHandling<RequestDto>(apiRequest);
        return response;
    }

    [Action("Get request extended metadata", Description = "Get extended metadata value for a given key")]
    public async Task<string> GetRequestMetadata([ActionParameter] GetRequest request, [ActionParameter] string Key)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{request.JobId}" +
                                               $"{ApiEndpoints.Requests}/{request.RequestId}");
        var response =  await Client.ExecuteWithErrorHandling(apiRequest);
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

        throw new PluginMisconfigurationException("No extended metadata was found for Request ID " + request.RequestId);
    }

    private async Task<string> CreateTranslationContent(string jobId, IEnumerable<string>? keys,
        IEnumerable<string>? values)
    {
        var listOfKeyValuePairs = CreateListOfKeyValuePairs(keys, values);
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}{ApiEndpoints.TranslationContent}", Method.Post)
            .WithJsonBody(new
            {
                fields = listOfKeyValuePairs
            });

        var response = await Client.ExecuteWithErrorHandling<TranslationContentDtoResponse>(apiRequest);
        return response.SourceContentId;
    }
}