namespace Apps.Lionbridge.Webhooks.Payload;

public class JobStatusUpdatedPayload
{
    public string StatusCode { get; set; }
    public bool Archived { get; set; }
    public bool Deleted { get; set; }
    public bool IsError { get; set; }
    public DateTime UpdateTime { get; set; }
    public string JobId { get; set; }
    public string EventType { get; set; }
    public string PayloadUuid { get; set; }
    public string ListenerId { get; set; }
}