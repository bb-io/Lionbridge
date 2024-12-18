﻿using Apps.Lionbridge.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Lionbridge.Models.Requests.Job;

public class CreateJobRequest
{
    [Display("Job name")]
    public string JobName { get; set; }
    
    public string? Description { get; set; }
    
    [Display("Provider ID")]
    [DataSource(typeof(ProviderDataSourceHandler))]
    public string? ProviderId { get; set; }
    
    [Display("Metadata keys", Description = "Extended metadata keys. For each specified key, a respective value " +
                                            "should be added in the 'Metadata values' input parameter.")]
    public IEnumerable<string>? MetadataKeys { get; set; }
    
    [Display("Metadata values", Description = "Extended metadata values. For each specified value, a respective " +
                                              "key should be added in the 'Metadata keys' input parameter.")]
    public IEnumerable<string>? MetadataValues { get; set; }
    
    [Display("Label keys", Description = "Label keys. For each specified key, a respective value should be added " +
                                         "in the 'Label values' input parameter.")]
    public IEnumerable<string>? LabelKeys { get; set; }
    
    [Display("Label values", Description = "Label values. For each specified value, a respective key should be " +
                                           "added in the 'Label keys' input parameter.")]
    public IEnumerable<string>? LabelValues { get; set; }

    [Display("Due date")]
    public DateTime? dueDate { get; set; }
}