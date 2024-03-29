﻿using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;

public class RequestStatuses : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "IN_TRANSLATION", "In translation" },
        { "REVIEW_TRANSLATION", "Review translation" },
        { "CANCELLED", "Cancelled" },
    };
}