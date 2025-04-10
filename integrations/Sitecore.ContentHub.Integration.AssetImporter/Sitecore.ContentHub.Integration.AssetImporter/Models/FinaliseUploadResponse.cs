namespace Sitecore.ContentHub.Integration.AssetImporter.Models
{
    public class FinaliseUploadResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public long? AssetId { get; set; }
        public string? AssetIdentifier { get; set; }
    }
}
