using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Requests.Request;

public class UpdateContentRequest
{
    [Display("Field names")]
    public IEnumerable<string>? FieldNames { get; set; }
    
    [Display("Field values")]
    public IEnumerable<string>? FieldValues { get; set; }
    
    [Display("Field comments")]
    public IEnumerable<string>? FieldComments { get; set; }
}