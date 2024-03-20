using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;

public class JobStatuses : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "IN_TRANSLATION", "In translation" },
        { "CANCELLED", "Cancelled" },
        { "SENT_TO_PROVIDER", "Sent to provider" },
        { "Update check", "Update check"}
    };
}