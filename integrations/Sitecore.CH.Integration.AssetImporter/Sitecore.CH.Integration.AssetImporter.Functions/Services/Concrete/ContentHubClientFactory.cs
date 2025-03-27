using Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract;
using Stylelabs.M.Sdk.WebClient.Authentication;
using Stylelabs.M.Sdk.WebClient;

namespace Sitecore.CH.Integration.AssetImporter.Functions.Services.Concrete
{
    class ContentHubClientFactory(IApplicationSettings applicationSettings) : IContentHubClientFactory
    {
        private readonly IApplicationSettings applicationSettings = applicationSettings;

        public IWebMClient CreateClient()
        {
            if (applicationSettings.ContentHubUrl == null)
                throw new ArgumentException("Content Hub Configuration not provided.");
            var endpoint = new Uri(applicationSettings.ContentHubUrl);

            var oAuth = new OAuthPasswordGrant
            {
                ClientId = applicationSettings.ContentHubClientId,
                ClientSecret = applicationSettings.ContentHubClientSecret,
                UserName = applicationSettings.ContentHubUsername,
                Password = applicationSettings.ContentHubPassword
            };

            return MClientFactory.CreateMClient(endpoint, oAuth);
        }
    }
}
