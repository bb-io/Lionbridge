﻿using Apps.Lionbridge.Models.Dtos;
using Newtonsoft.Json;

namespace Apps.Lionbridge.Models.Responses.TranslationContent;

public class TranslationContentDtoResponse
{
    [JsonProperty("sourcecontentId")]
    public string SourceContentId { get; set; }
    
    [JsonProperty("fields")]
    public List<FieldDto> Fields { get; set; }
}