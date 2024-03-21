using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Models.Responses.Provider;

public class ProvidersResponse : EmbeddedItemsWrapper<ProvidersWrapper>
{ 
    public string? Next { get; set; }
}

public record ProvidersWrapper(IEnumerable<ProviderDto> Providers);