using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Lionbridge;

public class LionbridgeApplication : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.LspPortal];
        set { }
    }
    
    public string Name
    {
        get => "Lionbridge";
        set { }
    }

    public T GetInstance<T>() => throw new NotImplementedException();
}