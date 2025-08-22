using Microsoft.Extensions.Logging;
using Sitecore.CH.Cli.Plugins.Base.Services.Abstract;
using Stylelabs.M.Sdk.Contracts.Querying.Generic;

namespace Sitecore.CH.Cli.Plugins.Base.Services.Concrete
{
    internal class IteratorHelper : IIteratorHelper
    {
        private readonly ILogger<IteratorHelper> logger;
        private readonly IClientHelper clientHelper;

        public IteratorHelper(ILogger<IteratorHelper> logger, IClientHelper clientHelper)
        {
            this.logger = logger;
            this.clientHelper = clientHelper;
        }

        public IAsyncEnumerable<TItem> IterateAsync<TQueryResult, TItem>(IQueryIterator<TQueryResult> iterator) where TQueryResult : IQueryResult<TItem>
        {
            return IterateAsyncInt<TQueryResult, TItem>(iterator).SelectMany(x => x.ToAsyncEnumerable());
        }

        private async IAsyncEnumerable<IList<TItem>> IterateAsyncInt<TQueryResult, TItem>(IQueryIterator<TQueryResult> iterator) where TQueryResult : IQueryResult<TItem>
        {
            while (iterator.CanMoveNext())
            {
                await clientHelper.Execute(_ => iterator.MoveNextAsync());
                yield return iterator.Current.Items;
            }
            yield break;
        }
    }
}
