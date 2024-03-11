using Apps.Lionbridge.Api;
using Apps.Lionbridge.Extensions;
using Apps.Lionbridge.Models.Dtos;
using Apps.Lionbridge.Models.Requests.Job;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Lionbridge.Actions;

[ActionList]
public class JobActions : LionbridgeInvocable
{
    public JobActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    #region Post

    [Action("Create job", Description = "Create a new translation job.")]
    public async Task<JobDto> CreateJob([ActionParameter] CreateJobRequest input)
    {
        var request = new LionbridgeRequest("/jobs", Method.Post)
            .WithJsonBody(new
            {
                jobName = input.JobName,
                description = input.Description,
                providerId = input.ProviderId,
                extendedMetadata = EnumerableExtensions.ToDictionary(input.MetadataKeys, input.MetadataValues),
                labels = EnumerableExtensions.ToDictionary(input.LabelKeys, input.LabelValues)
            });

        return await Client.ExecuteWithErrorHandling<JobDto>(request);
    }

    #endregion
}