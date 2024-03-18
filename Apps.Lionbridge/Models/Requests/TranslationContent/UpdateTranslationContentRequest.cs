using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Requests.TranslationContent;

public class UpdateTranslationContentRequest
{
    [Display("Field keys")]
    public IEnumerable<string> FieldKeys { get; set; }
    
    [Display("Field values")]
    public IEnumerable<string> FieldValues { get; set; }
}