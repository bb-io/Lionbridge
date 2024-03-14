using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Models.Responses.Request;

public class GetRequestsResponse
{
    public List<RequestDto> Requests { get; set; }
}