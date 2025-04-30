using Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract;
using Stylelabs.M.Sdk.WebClient.Authentication;
using Stylelabs.M.Sdk.WebClient;
using Microsoft.Extensions.Options;
using Sitecore.ContentHub.Integration.SearchConnector.Models.Options;

namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Concrete
{
    class ContentHubClientFactory(IOptions<ContentHubOptions> contentHubOptions) : IContentHubClientFactory
    {
        public IWebMClient CreateClient()
        {
            var endpoint = contentHubOptions.Value.BaseUrl;

            var oAuth = new OAuthPasswordGrant
            {
                ClientId = contentHubOptions.Value.ClientId,
                ClientSecret = contentHubOptions.Value.ClientSecret,
                UserName = contentHubOptions.Value.Username,
                Password = contentHubOptions.Value.Password
            };

            return MClientFactory.CreateMClient(endpoint, oAuth);
        }
    }
}
