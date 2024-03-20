using Apps.Lionbridge.Api;
using Apps.Lionbridge.Constants;
using Apps.Lionbridge.Models.Requests.Request;
using Apps.Lionbridge.Models.Responses.SourceFile;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class SourceFileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : LionbridgeInvocable(invocationContext)
{
    [Action("Retrieve file", Description = "Retrieve a file from specific request")]
    public async Task<RetrieveFileResponse> RetrieveFile([ActionParameter] GetRequest request)
    {
        string endpoint = $"{ApiEndpoints.Jobs}/{request.JobId}{ApiEndpoints.Requests}/{request.RequestId}{ApiEndpoints.RetrieveFile}";
        var apiRequest = new LionbridgeRequest(endpoint);
        
        var response = await Client.ExecuteWithErrorHandling(apiRequest);
        var bytes = response.RawBytes;
        
        if(bytes == null || bytes.Length == 0)
        {
            throw new Exception("Uploaded file is empty");
        }
        
        var requestModel = await GetRequest(request.JobId, request.RequestId);
        
        var memoryStream = new MemoryStream(bytes);
        string fileName = requestModel.FileName ?? requestModel.RequestName + ".xml"; // if there is no file name it means that request was created from source content
        string contentType = response.ContentType ?? MimeTypes.GetMimeType(fileName);
        var fileReference = await fileManagementClient.UploadAsync(memoryStream, contentType, fileName);
        
        return new RetrieveFileResponse { File = fileReference };
    }
}