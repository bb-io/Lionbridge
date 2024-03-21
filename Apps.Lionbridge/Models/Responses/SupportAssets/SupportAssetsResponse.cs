using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Models.Responses.SupportAssets;

public class SupportAssetsResponse : EmbeddedItemsWrapper<SupportAssetsWrapper>
{
    
}

public record SupportAssetsWrapper(IEnumerable<SupportAssetDto> SupportAssets);