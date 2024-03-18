using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Models.Responses.TranslationMemory;

public class TranslationMemoriesResponse : EmbeddedItemsWrapper<TranslationMemoriesWrapper>
{
    
}

public record TranslationMemoriesWrapper(IEnumerable<TranslationMemoryDto> tmupdates);