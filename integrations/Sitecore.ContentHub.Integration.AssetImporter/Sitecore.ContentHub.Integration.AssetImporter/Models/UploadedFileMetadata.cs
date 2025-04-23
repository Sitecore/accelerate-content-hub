namespace Sitecore.ContentHub.Integration.AssetImporter.Models
{
    public class UploadedFileMetadata
    {
        public required long Id { get; set; }
        public required string Identifier { get; set; }
        public required string Filename { get; set; }
    }
}
