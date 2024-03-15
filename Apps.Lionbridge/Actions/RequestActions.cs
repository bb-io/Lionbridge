using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.File;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Responses.Request;
using Apps.Lionbridge.Models.Responses.SourceFile;
using Apps.Lionbridge.Models.Responses.TranslationContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class RequestActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : LionbridgeInvocable(invocationContext)
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
        string sourceContentId =
            await CreateTranslationContent(jobRequest.JobId, request.FieldsKeys, request.FieldsValues);

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

    [Action("Create file request", Description = "Create a new translation request.")]
    public async Task<RequestDto> CreateFileRequest([ActionParameter] GetJobRequest jobRequest,
        [ActionParameter] AddSourceFileRequest sourceFileRequest)
    {
        var uploadResponse = await UploadFmsFile(jobRequest.JobId, sourceFileRequest);

        var metadata =
            EnumerableExtensions.ToDictionary(sourceFileRequest.MetadataKeys, sourceFileRequest.MetadataValues);
        var apiRequest =
            new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobRequest.JobId}" + ApiEndpoints.Requests + ApiEndpoints.Add,
                    Method.Post)
                .WithJsonBody(new
                {
                    requestName = sourceFileRequest.RequestName,
                    sourceNativeId = sourceFileRequest.SourceNativeId,
                    sourceNativeLanguageCode = sourceFileRequest.SourceNativeLanguageCode,
                    targetNativeIds = sourceFileRequest.TargetNativeIds,
                    targetNativeLanguageCodes = new List<string> { sourceFileRequest.TargetNativeLanguage },
                    wordCount = sourceFileRequest.WordCount,
                    extendedMetadata = metadata,
                    fmsFileId = uploadResponse.FmsFileId
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

    private async Task<string> CreateTranslationContent(string jobId, IEnumerable<string> keys,
        IEnumerable<string> values)
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

    private async Task<UploadSourceFileResponse> UploadFmsFile(string jobId, AddSourceFileRequest fileRequest)
    {
        string fileName = fileRequest.FileName ?? fileRequest.File.Name;

        string endpoint = $"{ApiEndpoints.Jobs}/{jobId}{ApiEndpoints.SourceFiles}?fileName={fileName}";
        var apiRequest = new LionbridgeRequest(endpoint, Method.Post);

        var response = await Client.ExecuteWithErrorHandling<UploadSourceFileResponse>(apiRequest);

        string fmsMultipartUrl = response.FmsPostMultipartUrl;

        var fileStream = await fileManagementClient.DownloadAsync(fileRequest.File);
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);

        var bytes = memoryStream.ToArray();

        var client = new RestClient(fmsMultipartUrl);
        var request = new RestRequest(string.Empty, Method.Post);
        request.AddFile("file", bytes, fileName);

        var uploadFileResponse = await client.ExecuteAsync(request);
        if (!uploadFileResponse.IsSuccessful)
        {
            throw new Exception("Failed to upload file to FMS on second step");
        }

        return response;
    }
}