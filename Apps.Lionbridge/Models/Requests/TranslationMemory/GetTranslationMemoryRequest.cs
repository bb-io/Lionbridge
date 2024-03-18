using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.TranslationMemory;

public class GetTranslationMemoryRequest
{
    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string LionBridgeJobId { get; set; }
    
    [Display("TM update ID"), DataSource(typeof(TranslationMemoryDataSourceHandler))]
    public string TmupdateId { get; set; }
}