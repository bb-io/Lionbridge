using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Webhooks.Responses;

public class JobStatusUpdatedResponse
{
    public JobDto Job { get; set; }

    public bool Archived { get; set; }
    
    public bool Deleted { get; set; }
}