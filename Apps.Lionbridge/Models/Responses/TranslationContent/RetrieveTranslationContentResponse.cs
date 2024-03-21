using Apps.Lionbridge.Models.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Responses.TranslationContent;

public class RetrieveTranslationContentResponse
{
    [Display("Request ID")]
    public string RequestId { get; set; }
    
    [Display("Source native ID")]
    public string SourceNativeId { get; set; }
    
    [Display("Source native language code")]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native ID")]
    public string TargetNativeId { get; set; }
    
    [Display("Target native language code")]
    public string TargetNativeLanguageCode { get; set; }
    
    [Display("Field keys")]
    public IEnumerable<string> FieldKeys { get; set; }
    
    [Display("Field values")]
    public IEnumerable<string> FieldValues { get; set; }

    public RetrieveTranslationContentResponse(RetrieveTranslationContentDto dto)
    {
        RequestId = dto.RequestId;
        SourceNativeId = dto.SourceNativeId;
        SourceNativeLanguageCode = dto.SourceNativeLanguageCode;
        TargetNativeId = dto.TargetNativeId;
        TargetNativeLanguageCode = dto.TargetNativeLanguageCode;
        FieldKeys = dto.TargetContent.Select(x => x.Key);
        FieldValues = dto.TargetContent.Select(x => x.Value);
    }
}

public class RetrieveTranslationContentMultiplyResponse : EmbeddedItemsWrapper<RetrieveTranslationContentResponseWrapper>
{
    
}

public record RetrieveTranslationContentResponseWrapper(IEnumerable<RetrieveTranslationContentDto> TranslationContent);
