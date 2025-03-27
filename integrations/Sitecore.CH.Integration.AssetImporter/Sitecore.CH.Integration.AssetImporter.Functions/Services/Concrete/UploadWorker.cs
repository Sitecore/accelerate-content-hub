using Microsoft.Extensions.Logging;
using Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract;

namespace Sitecore.CH.Integration.AssetImporter.Functions.Services.Concrete
{
    class UploadWorker(ILoggerFactory loggerFactory, IStorageService storageService, IUploadService uploadService) : IUploadWorker
    {
        private readonly ILogger logger = loggerFactory.CreateLogger<UploadWorker>();
        private readonly IStorageService storageService = storageService;
        private readonly IUploadService uploadService = uploadService;

        public async Task ProcessUploads()
        {
            var files = await storageService.GetFilesAsync();
            foreach (var file in files)
            {
                var content = await storageService.GetFileContentsAsync(file);
                var success = await uploadService.Upload(file, content);
                if (success)
                    await storageService.RemoveFileAsync(file);
                else
                    logger.LogWarning("Failed to upload file {file}", file);
            }
        }
    }
}
