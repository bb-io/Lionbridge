using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.SupportAssets;

public class GetSupportAssetRequest
{
    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string LionBridgeJobId { get; set; }
    
    [Display("Support asset ID"), DataSource(typeof(SupportAssetDataSourceHandler))]
    public string SupportAssetId { get; set; }
}