using Apps.Lionbridge.Models.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Responses.TranslationMemory;

public class TranslationMemoryResponse
{
    [Display("TM update ID")]
    public string TmupdateId { get; set; }
    
    [Display("Job ID")]
    public string JobId { get; set; }
    
    [Display("File ID")]
    public string FileId { get; set; }
    
    [Display("File type")]
    public string FileType { get; set; }
    
    [Display("Filename")]
    public string FileName { get; set; }
    
    [Display("Source native language code")]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native language code")]
    public string TargetNativeLanguageCode { get; set; }
    
    [Display("Extended metadata keys")]
    public IEnumerable<string> ExtendedMetadataKeys { get; set; }
    
    [Display("Extended metadata values")]
    public IEnumerable<string> ExtendedMetadataValues { get; set; }
    
    public TranslationMemoryResponse(TranslationMemoryDto translationMemoryDto)
    {
        TmupdateId = translationMemoryDto.TmupdateId;
        JobId = translationMemoryDto.JobId;
        FileId = translationMemoryDto.FileId;
        FileType = translationMemoryDto.FileType;
        FileName = translationMemoryDto.FileName;
        SourceNativeLanguageCode = translationMemoryDto.SourceNativeLanguageCode;
        TargetNativeLanguageCode = translationMemoryDto.TargetNativeLanguageCode;
        ExtendedMetadataKeys = translationMemoryDto.ExtendedMetadata.Keys.ToArray();
        ExtendedMetadataValues = translationMemoryDto.ExtendedMetadata.Values.ToArray();
    }
}