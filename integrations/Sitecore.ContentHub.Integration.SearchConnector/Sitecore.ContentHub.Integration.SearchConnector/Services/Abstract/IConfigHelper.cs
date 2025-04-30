using Sitecore.ContentHub.Integration.SearchConnector.Models.Config;

namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract
{
    public interface IConfigHelper
    {
        public IEnumerable<CultureMap> CultureMaps { get; }
        public IEnumerable<DefinitionMap> DefinitionMaps { get; }
    }
}
