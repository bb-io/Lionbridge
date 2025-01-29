using Apps.Lionbridge.Connections;
using Apps.Lionbridge.Webhooks.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Lionbridge.Base;

namespace Tests.Lionbridge;

[TestClass]
public class WebhookTests : TestBase
{
    public const string webhookUrl = "https://webhook.site/113325da-06d7-4f51-8c18-5e63fe9007ad";

    [TestMethod]
    public async Task Subscribes_new_request_webhook()
    {
        var handler = new RequestStatusUpdatedHandler(InvocationContext, 
            new Apps.Lionbridge.Webhooks.Inputs.GetRequestsInput { });

        await handler.SubscribeAsync(InvocationContext.AuthenticationCredentialsProviders, 
            new Dictionary<string, string>() 
            {
                { "payloadUrl", webhookUrl }
            });
    }

    [TestMethod]
    public async Task Subscribes_new_job_webhook()
    {
        var handler = new JobStatusUpdatedHandler(InvocationContext, 
        new Apps.Lionbridge.Webhooks.Inputs.JobStatusUpdatedInput 
        { });

        await handler.SubscribeAsync(InvocationContext.AuthenticationCredentialsProviders,
            new Dictionary<string, string>()
            {
                { "payloadUrl", webhookUrl }
            });
    }

    [TestMethod]
    public async Task AllRequestInReview_on_unsubscribe_works()
    {
        var handler = new AllRequestsInReviewHandler(InvocationContext, new Apps.Lionbridge.Webhooks.Inputs.CompletedRequestsInput { JobId = "MGhj2DcVl9" });

        var result = await handler.OnWebhookSubscribedAsync();

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result.Result.Requests.All(x => x.StatusCode == "REVIEW_TRANSLATION"));
    }
}
