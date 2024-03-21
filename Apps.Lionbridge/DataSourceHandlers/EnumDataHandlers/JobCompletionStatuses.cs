using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;

public class JobCompletionStatuses : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "IN_TRANSLATION", "In translation" },
        { "COMPLETED", "Complete" },
    };
}