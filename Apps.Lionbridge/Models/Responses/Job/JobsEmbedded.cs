using Apps.Lionbridge.Models.Dtos;
using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Responses.Job
{
    public class JobsEmbedded
    {
        [JsonProperty("jobs")]
        public IEnumerable<JobDto> Jobs { get; set; }
    }

}
