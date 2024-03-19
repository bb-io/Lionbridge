using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Models.Responses.StatusUpdates;

public class StatusUpdatesResponse : EmbeddedItemsWrapper<StatusUpdatesWrapper>
{
    
}

public record StatusUpdatesWrapper(IEnumerable<StatusUpdateDto> statusupdates);