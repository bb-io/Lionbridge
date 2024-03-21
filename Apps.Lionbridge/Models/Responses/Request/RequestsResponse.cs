using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Models.Responses.Request;

public class RequestsResponse : EmbeddedItemsWrapper<RequestsWrapper>
{
}

public record RequestsWrapper(IEnumerable<RequestDto> Requests);