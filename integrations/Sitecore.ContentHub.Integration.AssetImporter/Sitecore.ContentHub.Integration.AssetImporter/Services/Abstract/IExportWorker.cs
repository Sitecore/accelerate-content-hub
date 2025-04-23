using Sitecore.ContentHub.Integration.AssetImporter.Models;

namespace Sitecore.ContentHub.Integration.AssetImporter.Services.Abstract
{
    public interface IExportWorker
    {
        Task<IEnumerable<UploadedFileMetadata>> GenerateMetadataExport();

        Task<MemoryStream> GenerateMetadataExcel();
    }
}
