using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;

public class JobStatusDataHandler : IStaticDataSourceItemHandler
{

    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>()
        {
            new("CREATED", "Created" ),
            new("SENDING", "Sending" ),
            new("SENT_TO_PROVIDER", "Sent to provider" ),
            new("IN_TRANSLATION", "In translation" ),
            new("CANCELLED", "Cancelled" ),
            new("COMPLETED", "Complete"),
        };
    }
}