using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge;

public class LionbridgeApplication : IApplication
{
    public string Name
    {
        get => "Lionbridge";
        set { }
    }

    public T GetInstance<T>() => throw new NotImplementedException();
}