using Apps.Lionbridge.Actions;
using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Apps.Lionbridge.Models.Requests.File;
using Apps.Lionbridge.Models.Requests.Job;
using Apps.Lionbridge.Webhooks.Handlers;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Lionbridge.Base;

namespace Tests.Lionbridge;

[TestClass]
public class JobRequestsTests : TestBase
{
    public const string JobName = "Test name";
    public const string FileName = "test.html";

    [TestMethod]
    public async Task Create_new_job_with_one_request()
    {
        var jobActions = new JobActions(InvocationContext);
        var requestActions = new RequestActions(InvocationContext, FileManager);

        var newJob = await jobActions.CreateJob(new CreateJobRequest { JobName = JobName });

        Assert.AreEqual(JobName, newJob.JobName);

        var requests = await requestActions.CreateFileRequest(
            new GetJobRequest { JobId = newJob.JobId }, 
            new AddSourceFileRequest { 
                File = new FileReference { Name = FileName },
                SourceNativeLanguageCode = "en-us",
                TargetNativeLanguage = new List<string>
                {
                    "nl-NL",
                }
            }
        );

        Assert.IsTrue(requests.Requests.Count() == 1);
        
        foreach( var request in requests.Requests )
        {
            Console.WriteLine(JsonConvert.SerializeObject(request, Formatting.Indented));
        }
    }

    [TestMethod]
    public async Task Create_new_job_with_100_requests()
    {
        var jobActions = new JobActions(InvocationContext);
        var requestActions = new RequestActions(InvocationContext, FileManager);
        var languageDataHandler = new LanguageDataHandler();
        var languages = languageDataHandler.GetData();

        var newJob = await jobActions.CreateJob(new CreateJobRequest { JobName = JobName });

        Assert.AreEqual(JobName, newJob.JobName);

        var requests = await requestActions.CreateFileRequest(
            new GetJobRequest { JobId = newJob.JobId },
            new AddSourceFileRequest
            {
                File = new FileReference { Name = FileName },
                SourceNativeLanguageCode = "en-us",
                TargetNativeLanguage = languages.Select(x => x.Value).Take(100),
            }
        );        

        Console.WriteLine($"Total count: {requests.Requests.Count()}");

        foreach (var request in requests.Requests)
        {
            Console.WriteLine(JsonConvert.SerializeObject(request, Formatting.Indented));
        }

        Assert.IsTrue(requests.Requests.Count() == 100);
    }
}
