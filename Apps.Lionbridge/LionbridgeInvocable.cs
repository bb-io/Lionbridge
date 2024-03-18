using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests;
using Apps.Lionbridge.Models.Requests.File;
using Apps.Lionbridge.Models.Responses.SourceFile;
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
}