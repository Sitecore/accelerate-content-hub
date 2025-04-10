using Sitecore.ContentHub.Integration.AssetImporter.Services.Abstract;

namespace Sitecore.ContentHub.Integration.AssetImporter.Services.Concrete
{
    class ApplicationSettings : IApplicationSettings
    {
        public string? AzureStorageContainerName { get; set; }
        public string? ContentHubUrl { get; set; }
        public string? ContentHubClientId { get; set; }
        public string? ContentHubClientSecret { get; set; }
        public string? ContentHubUsername { get; set; }
        public string? ContentHubPassword { get; set; }

        public ApplicationSettings()
        {
            AzureStorageContainerName = Environment.GetEnvironmentVariable("AzureStorageContainerName");
            ContentHubUrl = Environment.GetEnvironmentVariable("ContentHubUrl");
            ContentHubClientId = Environment.GetEnvironmentVariable("ContentHubClientId");
            ContentHubClientSecret = Environment.GetEnvironmentVariable("ContentHubClientSecret");
            ContentHubUsername = Environment.GetEnvironmentVariable("ContentHubUsername");
            ContentHubPassword = Environment.GetEnvironmentVariable("ContentHubPassword");
        }
    }
}
