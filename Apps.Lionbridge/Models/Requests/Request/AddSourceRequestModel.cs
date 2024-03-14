using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Lionbridge.Models.Requests.Request;

public class AddSourceRequestModel : AddRequestBaseModel
{
    [Display("Target native language")]
    public string TargetNativeLanguage { get; set; }

    [Display("Fields keys", Description = "Fields keys to be added to the request.")]
    public IEnumerable<string> FieldsKeys { get; set; }
    
    [Display("Fields values", Description = "Fields values to be added to the request.")]
    public IEnumerable<string> FieldsValues { get; set; }
}