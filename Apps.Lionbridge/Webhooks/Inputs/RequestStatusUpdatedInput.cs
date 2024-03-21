using Apps.Lionbridge.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Webhooks.Inputs;

public class RequestStatusUpdatedInput
{
    [Display("Status codes"), DataSource(typeof(RequestStatuses))]
    public IEnumerable<string>? StatusCodes { get; set; }
}