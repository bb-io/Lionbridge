using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Webhooks.Responses;

public class RequestStatusUpdatedResponse
{
    public IEnumerable<RequestDto> Requests { get; set; }
    
    public JobDto Job { get; set; }
}