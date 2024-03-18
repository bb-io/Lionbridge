using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Responses.TranslationContent;

public class TranslationContentResponse
{
    [Display("Source Content ID")]
    public string SourceContentId { get; set; }
    
    [Display("Field keys")]
    public IEnumerable<string> FieldKeys { get; set; }
    
    [Display("Field values")]
    public IEnumerable<string> FieldValues { get; set; }

    public TranslationContentResponse(TranslationContentDtoResponse dto)
    {
        SourceContentId = dto.SourceContentId;
        FieldKeys = dto.Fields.Select(f => f.Key);
        FieldValues = dto.Fields.Select(f => f.Value);
    }
}