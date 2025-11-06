using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Requests.TranslationContent;
using Apps.Lionbridge.Models.Responses.TranslationContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList("Translation content")]
public class TranslationContentActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : LionbridgeInvocable(invocationContext)
{
    [Action("Get translation content", Description = "Access the original content submitted for translation in a job")]
    public async Task<TranslationContentResponse> GetAllTranslationContent([ActionParameter] GetJobRequest request,
        [ActionParameter, Display("Source content ID")] string sourceContentId)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.TranslationContent}/{sourceContentId}";
        var apiRequest = new LionbridgeRequest(endpoint);

        var dto = await Client.ExecuteWithErrorHandling<TranslationContentDtoResponse>(apiRequest);
        return new TranslationContentResponse(dto);
    }
    
    [Action("Update translation content", Description = "Update the original content submitted for translation")]
    public async Task<TranslationContentResponse> UpdateTranslationContent([ActionParameter] GetJobRequest request,
        [ActionParameter, Display("Source content ID")] string sourceContentId,
        [ActionParameter] UpdateTranslationContentRequest updateTranslationContentRequest)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.TranslationContent}/{sourceContentId}";
        var keyValuePairs = CreateListOfKeyValuePairs(updateTranslationContentRequest.FieldKeys, updateTranslationContentRequest.FieldValues);
        
        var apiRequest = new LionbridgeRequest(endpoint, Method.Put)
            .AddJsonBody(new
            {
                fields = keyValuePairs
            });
        
        var dto = await Client.ExecuteWithErrorHandling<TranslationContentDtoResponse>(apiRequest);
        return new TranslationContentResponse(dto);
    }
    
    [Action("Retrieve source content", Description = "Download the translated content for one or more translation requests")]
    public async Task<RetrieveTranslationContentForRequestResponse> RetrieveTranslationContent([ActionParameter] GetRequest request)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.Requests}/{request.RequestId}{ApiEndpoints.Retrieve}";
        
        var apiRequest = new LionbridgeRequest(endpoint);
        var dto = await Client.ExecuteWithErrorHandling<RetrieveTranslationContentMultiplyResponse>(apiRequest);
        return new RetrieveTranslationContentForRequestResponse(dto);
    }
}