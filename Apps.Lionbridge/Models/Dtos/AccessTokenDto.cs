using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Dtos;

public record AccessTokenDto
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
}