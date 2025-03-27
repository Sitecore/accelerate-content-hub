namespace Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract
{
    interface IApplicationSettings
    {
        string? AzureStorageContainerName { get; }
        string? ContentHubUrl { get; }
        string? ContentHubClientId { get; }
        string? ContentHubClientSecret { get; }
        string? ContentHubUsername { get; }
        string? ContentHubPassword { get; }
    }
}
