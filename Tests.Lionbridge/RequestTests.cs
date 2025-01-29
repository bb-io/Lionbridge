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
public class RequestTests : TestBase
{
    public const string JobId = "MGhj2DcVl9";

    [TestMethod]
    public async Task Search_requests_works()
    {
        var actions = new RequestActions(InvocationContext, FileManager);

        var result = await actions.GetRequests(new Apps.Lionbridge.Models.Requests.Request.GetRequestsAsOptional { JobId = JobId });

        Assert.IsTrue(result.Requests.Count() > 0);

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

    }
}
