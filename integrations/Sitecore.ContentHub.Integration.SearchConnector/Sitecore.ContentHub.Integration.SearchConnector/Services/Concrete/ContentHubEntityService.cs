using Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;

namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Concrete
{
    class ContentHubEntityService(IContentHubClientHelper contentHubClientHelper) : IContentHubEntityService
    {
        public Task<IEntity> GetEntityById(long id, IEntityLoadConfiguration? loadConfiguration = null)
        {
            return contentHubClientHelper.Execute(client => client.Entities.GetAsync(id, loadConfiguration));
        }

        public Task<IEntity> GetEntityByIdentifier(string identifier, IEntityLoadConfiguration? loadConfiguration = null)
        {
            return contentHubClientHelper.Execute(client => client.Entities.GetAsync(identifier, loadConfiguration));
        }

        public IDictionary<string, object> GetPropertyValueForCultures(IEntity entity, string propertyName)
        {
            var isMultiLanguage = entity.GetProperty(propertyName).IsMultiLanguage;
            return entity.Cultures.ToDictionary(c => c.Name, c => isMultiLanguage ? entity.GetPropertyValue(propertyName, c) : entity.GetPropertyValue(propertyName));
        }
    }
}
