using Apps.Lionbridge.Actions;
using Apps.Lionbridge.Models.Requests.File;
using Apps.Lionbridge.Models.Requests.Job;
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
public class JobTests : TestBase
{
    public const string JobId = "FZwlbFcVlD";

    [TestMethod]
    public async Task Complete_job_works()
    {
        var actions = new JobActions(InvocationContext);

        var result = await actions.CompleteJob(new GetJobRequest { JobId = JobId });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

        Assert.IsTrue(result.StatusCode == "COMPLETED");

    }
}
