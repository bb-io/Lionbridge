using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests;
using Apps.Lionbridge.Models.Requests.File;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Responses.Request;
using Apps.Lionbridge.Models.Responses.SourceFile;
using Apps.Lionbridge.Models.Responses.StatusUpdates;
using Apps.Lionbridge.Models.Responses.TranslationContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Lionbridge;

public class LionbridgeInvocable : BaseInvocable
{
    protected readonly LionbridgeClient Client;

    protected LionbridgeInvocable(InvocationContext invocationContext) : base(invocationContext) 
    {
        Client = new(InvocationContext.AuthenticationCredentialsProviders);
    }
    
    protected async Task<RequestDto> GetRequest(string jobId, string requestId)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Jobs}/{jobId}" +
                                               $"{ApiEndpoints.Requests}/{requestId}");
        return await Client.ExecuteWithErrorHandling<RequestDto>(apiRequest);
    }
    
    protected async Task<UploadSourceFileResponse> UploadFmsFile(string jobId, AddFileRequest fileRequest, IFileManagementClient fileManagementClient)
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
    
    protected List<FieldDto> CreateListOfKeyValuePairs(IEnumerable<string> keys, IEnumerable<string> values)
    {
        if(keys.Count() != values.Count())
        {
            throw new Exception("Keys and values count must be equal");
        }
        
        return keys.Zip(values, (k, v) => new FieldDto { Key = k, Value = v }).ToList();
    }
    
    protected async Task WaitUntilJobIsCompleted(string jobId, string providerId, string[] requestIds)
    {
        var apiRequest = new LionbridgeRequest($"{ApiEndpoints.Providers}/{providerId}{ApiEndpoints.StatusUpdates}" );

        StatusUpdateDto? statusUpdate = null;
        var foundRequestIds = new List<string>();
        
        int attempts = 0;
        do
        {
            var statusUpdates = await Client.ExecuteWithErrorHandling<StatusUpdatesResponse>(apiRequest);
            statusUpdate = statusUpdates.Embedded.statusupdates.FirstOrDefault(j => j.JobId == jobId);

            if (statusUpdate != null)
            {
                if (statusUpdate.StatusCode == "IN_TRANSLATION" || statusUpdate.StatusCode == "CANCELLED")
                {
                    foundRequestIds.AddRange(statusUpdate.RequestIds);
                }
            }
            
            if (foundRequestIds.Count == requestIds.Length)
            {
                return;
            }
            
            await Task.Delay(5000);
            attempts++;
        } while (attempts < 40);
        
        throw new Exception("Job did not complete in time");
    }
    
    protected async Task<GetRequestsResponse> GetRequests(string jobId, IEnumerable<string>? requestIds)
    {
        RestRequest apiRequest = new LionbridgeRequest(
            $"{ApiEndpoints.Jobs}/{jobId}" + ApiEndpoints.Requests,
            Method.Get);

        var response = await Client.ExecuteWithErrorHandling<RequestsResponse>(apiRequest);
        var requests = response.Embedded.Requests.ToList();
        if (requestIds != null)
        {
            var requestIdsAsArray = requestIds as string[] ?? requestIds.ToArray();
            if (requestIdsAsArray.Any())
            {
                requests = requests.Where(x => requestIdsAsArray.Contains(x.RequestId)).ToList();
            }
        }

        return new GetRequestsResponse { Requests = requests };
    }
}