namespace Apps.Lionbridge.Webhooks.Payload;

public class RequestStatusUpdatedPayload
{
    public List<string>? RequestIds { get; set; }
    
    public string StatusCode { get; set; }
    
    public string Message { get; set; }
    
    public bool IsError { get; set; }
    
    public DateTime UpdateTime { get; set; }
    
    public string JobId { get; set; }
    
    public string EventType { get; set; }
    
    public string PayloadUuid { get; set; }
    
    public string ListenerId { get; set; }
}