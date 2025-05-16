using Sitecore.ContentHub.Integration.SearchConnector.Constants;
using Stylelabs.M.Base.Querying;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Sdk.Search;

namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract
{
    public interface IContentHubSearchService
    {
        Task<IEnumerable<IEntity>> SearchAfter(QueryNodeFilter filter, IEnumerable<Sorting>? sorting = null, IEntityLoadConfiguration? entityLoadConfiguration = null, int take = ApiConstants.ContentHubClient.SearchAfterDefaultTake);
    }
}
