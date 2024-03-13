using Apps.Lionbridge.Models.Dtos;

namespace Apps.Lionbridge.Models.Responses.Job;

public class JobsResponse : EmbeddedItemsWrapper<JobsWrapper>
{ }

public record JobsWrapper(IEnumerable<JobDto> Jobs);