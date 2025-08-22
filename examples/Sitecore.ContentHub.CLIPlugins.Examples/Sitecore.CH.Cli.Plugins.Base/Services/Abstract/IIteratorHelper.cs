using Stylelabs.M.Sdk.Contracts.Querying.Generic;

namespace Sitecore.CH.Cli.Plugins.Base.Services.Abstract
{
    interface IIteratorHelper
    {
        IAsyncEnumerable<TItem> IterateAsync<TQueryResult, TItem>(IQueryIterator<TQueryResult> iterator) where TQueryResult : IQueryResult<TItem>;
    }
}
