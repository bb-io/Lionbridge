using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Lionbridge.Webhooks.Inputs;
public class CompletedRequestsInput
{
    [Display("Job ID"), DataSource(typeof(JobDataSourceHandler))]
    public string JobId { get; set; }
}
