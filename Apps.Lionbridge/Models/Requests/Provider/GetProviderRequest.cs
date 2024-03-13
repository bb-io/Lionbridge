using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.Provider;

public class GetProviderRequest
{
    [Display("Provider ID"), DataSource(typeof(ProviderDataSourceHandler))]
    public string ProviderId { get; set; }
}