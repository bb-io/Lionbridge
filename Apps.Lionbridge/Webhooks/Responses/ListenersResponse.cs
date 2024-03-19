using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Webhooks.Responses;

public class ListenersResponse : EmbeddedItemsWrapper<ListenersWrapper>
{
    
}

public record ListenersWrapper(IEnumerable<ListenerDto> Listeners);