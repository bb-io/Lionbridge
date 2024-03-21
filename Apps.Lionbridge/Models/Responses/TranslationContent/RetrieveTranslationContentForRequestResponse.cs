using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Responses.TranslationContent;

public class RetrieveTranslationContentForRequestResponse
{
    [Display("Source content")]
    public IEnumerable<RetrieveTranslationContentResponse> SourceContent { get; set; }

    public RetrieveTranslationContentForRequestResponse(RetrieveTranslationContentMultiplyResponse response)
    {
        SourceContent = response.Embedded.TranslationContent.Select(x => new RetrieveTranslationContentResponse(x));
    }
}