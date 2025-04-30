using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;

namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract
{
    public interface IContentHubEntityService
    {
        Task<IEntity> GetEntityById(long id, IEntityLoadConfiguration? loadConfiguration = null);

        Task<IEntity> GetEntityByIdentifier(string identifier, IEntityLoadConfiguration? loadConfiguration = null);

        IDictionary<string, object> GetPropertyValueForCultures(IEntity entity, string propertyName);
    }
}
