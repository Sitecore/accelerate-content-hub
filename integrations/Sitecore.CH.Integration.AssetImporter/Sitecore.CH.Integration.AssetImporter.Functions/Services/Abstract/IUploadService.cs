namespace Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract
{
    public interface IUploadService
    {
        Task<bool> Upload(string name, byte[] content);
    }
}
