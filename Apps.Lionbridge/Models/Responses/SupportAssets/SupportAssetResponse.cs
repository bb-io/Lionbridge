using Apps.Lionbridge.Models.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Lionbridge.Models.Responses.SupportAssets;

public class SupportAssetResponse
{
    [Display("Support asset ID")] 
    public string SupportAssetId { get; set; }
    
    [Display("File ID")]
    public string FileId { get; set; }
    
    [Display("Job ID")]
    public string JobId { get; set; }
    
    [Display("Filename")]
    public string Filename { get; set; }
    
    [Display("Description")]
    public string Description { get; set; }
    
    [Display("Source native IDs")]
    public string[] SourceNativeIds { get; set; }
    
    [Display("Source native language code")]
    public string SourceNativeLanguageCode { get; set; }
    
    [Display("Target native language codes")]
    public string[] TargetNativeLanguageCodes { get; set; }
    
    [Display("Created date")]
    public DateTime CreatedDate { get; set; }
    
    [Display("Extended metadata keys")]
    public IEnumerable<string> ExtendedMetadataKeys { get; set; }
    
    [Display("Extended metadata values")]
    public IEnumerable<string> ExtendedMetadataValues { get; set; }

    public SupportAssetResponse(SupportAssetDto supportAssetDto)
    {
        SupportAssetId = supportAssetDto.SupportAssetId;
        FileId = supportAssetDto.FileId;
        JobId = supportAssetDto.JobId;
        Filename = supportAssetDto.Filename;
        Description = supportAssetDto.Description;
        SourceNativeIds = supportAssetDto.SourceNativeIds;
        SourceNativeLanguageCode = supportAssetDto.SourceNativeLanguageCode;
        TargetNativeLanguageCodes = supportAssetDto.TargetNativeLanguageCodes;
        CreatedDate = supportAssetDto.CreatedDate;
        ExtendedMetadataKeys = supportAssetDto.ExtendedMetadata.Keys.ToArray();
        ExtendedMetadataValues = supportAssetDto.ExtendedMetadata.Values.ToArray();
    }
}

