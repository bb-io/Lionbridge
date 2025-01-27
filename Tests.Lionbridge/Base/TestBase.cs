using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Lionbridge.Base;
public class TestBase
{
    public IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

    public InvocationContext InvocationContext { get; set; }

    public FileManager FileManager { get; set; }

    public string BridgeToken { get; set; }

    public TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Creds = config.GetSection("ConnectionDefinition").GetChildren().Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value)).ToList();
        var folderLocation = config.GetSection("TestFolder").Value;
        BridgeToken = config.GetSection("BridgeToken").Value;

        InvocationContext = new InvocationContext
        {
            AuthenticationCredentialsProviders = Creds,
            UriInfo = new UriInfo
            {
                BridgeServiceUrl = new Uri("https://bridge.blackbird.io/api"),
            }
        };

        FileManager = new FileManager(folderLocation);
    }
}
