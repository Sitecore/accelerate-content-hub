using Azure.Storage.Blobs.Models;

namespace Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract
{
    public interface IStorageService
    {
        Task<IEnumerable<string>> GetFilesAsync();

        Task<byte[]> GetFileContentsAsync(string fileName);

        Task RemoveFileAsync(string fileName);
    }
}
