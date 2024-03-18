using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.SupportAssets;
using Apps.Lionbridge.Models.Responses.SupportAssets;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class SupportAssetsActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : LionbridgeInvocable(invocationContext)
{
    [Action("Get support asset", Description = "Get a support asset.")]
    public async Task<SupportAssetResponse> GetSupportAsset([ActionParameter] GetSupportAssetRequest request)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.LionBridgeJobId}{ApiEndpoints.SupportAssets}/{request.SupportAssetId}";
        var apiRequest = new LionbridgeRequest(endpoint);

        var dto = await Client.ExecuteWithErrorHandling<SupportAssetDto>(apiRequest);
        return new SupportAssetResponse(dto);
    }

    [Action("Delete support asset", Description = "Delete a support asset.")]
    public async Task DeleteSupportAsset([ActionParameter] GetSupportAssetRequest request)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.LionBridgeJobId}{ApiEndpoints.SupportAssets}/{request.SupportAssetId}";
        var apiRequest = new LionbridgeRequest(endpoint, Method.Delete);

        await Client.ExecuteWithErrorHandling(apiRequest);
    }

    [Action("Add support asset", Description = "Add a support asset to a job")]
    public async Task<SupportAssetResponse> AddSupportAsset([ActionParameter] GetJobRequest request,
        [ActionParameter] AddSupportAssetRequest addSupportAssetRequest,
        [ActionParameter] AddFileRequest fileRequest)
    {
        var uploadResponse = await UploadFmsFile(request.JobId, fileRequest, fileManagementClient);

        string endpoint = $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.SupportAssets}";
        var metadata = EnumerableExtensions.ToDictionary(addSupportAssetRequest.ExtendedMetadataKeys, addSupportAssetRequest.ExtendedMetadataValues);
        var apiRequest = new LionbridgeRequest(endpoint, Method.Post)
            .AddJsonBody(new
            {
                fmsFileId = uploadResponse.FmsFileId,
                description = addSupportAssetRequest.Description,
                sourceNativeIds = addSupportAssetRequest.SourceNativeIds,
                sourceNativeLanguageCode = addSupportAssetRequest.SourceNativeLanguageCode,
                targetNativeLanguageCodes = addSupportAssetRequest.TargetNativeLanguageCodes ?? [fileRequest.TargetNativeLanguage],
                extendedMetadata = metadata
            });

        var dto = await Client.ExecuteWithErrorHandling<SupportAssetDto>(apiRequest);
        return new SupportAssetResponse(dto);
    }
}