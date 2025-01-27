using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;

public class RequestStatusDataHandler : IStaticDataSourceItemHandler
{

    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>()
        {
         //   new("CREATED", "Created" ),
         //   new("SENDING", "Sending" ),
         //   new("SENT_TO_PROVIDER", "Sent to provider" ),
            new("IN_TRANSLATION", "In translation" ),
            new("REVIEW_TRANSLATION", "Ready for review" ),
          //  new("TRANSLATION_REJECTED", "Translation rejected" ),
         //   new("TRANSLATION_APPROVED", "Translation approved"),
            new("CANCELLED", "Cancelled"),
        };
    }
}

//Statuses commented out trigger error when subscribing to listener