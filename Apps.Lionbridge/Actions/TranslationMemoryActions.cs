using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.TranslationMemory;
using Apps.Lionbridge.Models.Responses.TranslationMemory;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class TranslationMemoryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : LionbridgeInvocable(invocationContext)
{
    [Action("Add translation memory", Description = "Add a translation memory to a job")]
    public async Task<TranslationMemoryResponse> AddTranslationMemory([ActionParameter] GetJobRequest request,
        [ActionParameter] AddTranslationMemoryRequest addTranslationMemoryRequest,
        [ActionParameter] AddFileRequest fileRequest)
    {
        var uploadResponse = await UploadFmsFile(request.JobId, fileRequest, fileManagementClient);
        string endpoint = $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.TranslationMemories}";
        var extendedMetadata = EnumerableExtensions.ToDictionary(addTranslationMemoryRequest.ExtendedMetadataKeys, addTranslationMemoryRequest.ExtendedMetadataValues);
        
        var apiRequest = new LionbridgeRequest(endpoint, Method.Post)
            .AddJsonBody(new
            {
                fmsFileId = uploadResponse.FmsFileId,
                sourceNativeLanguageCode = addTranslationMemoryRequest.SourceNativeLanguageCode,
                targetNativeLanguageCode = fileRequest.TargetNativeLanguage,
                extendedMetadata = extendedMetadata
            });
        
        var dto = await Client.ExecuteWithErrorHandling<TranslationMemoryDto>(apiRequest);
        return new TranslationMemoryResponse(dto);
    }

    [Action("Get translation memory", Description = "Get a translation memory.")]
    public async Task<TranslationMemoryResponse> GetTranslationMemory(
        [ActionParameter] GetTranslationMemoryRequest request)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.LionBridgeJobId}{ApiEndpoints.TranslationMemories}/{request.TmupdateId}";
        var apiRequest = new LionbridgeRequest(endpoint);
        var dto = await Client.ExecuteWithErrorHandling<TranslationMemoryDto>(apiRequest);
        return new TranslationMemoryResponse(dto);
    }
}