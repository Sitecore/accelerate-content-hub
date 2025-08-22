using Sitecore.CH.Cli.Plugins.Base.Services.Abstract;

namespace Sitecore.CH.Cli.Plugins.Base.Services.Concrete
{
    internal class BatchTaskHelper : IBatchTaskHelper
    {
        public IEnumerable<TOut> Batch<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, TOut> operation)
        {
            var batchedItems = CreateBatches(items, batchSize);
            return batchedItems.Select(batchOfItems => {
                return operation(batchOfItems);
            });
        }

        public IEnumerable<TOut> BatchMany<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, IEnumerable<TOut>> operation)
        {
            return CreateBatches(items, batchSize)
                .Select(batchOfItems => operation(batchOfItems)).SelectMany(x => x);
        }

        public async Task BatchAsync<TItem>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, Task> asyncOperation)
        {
            var tasks = Batch(batchSize, items, asyncOperation);
            await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<TOut>> BatchAsync<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, Task<TOut>> asyncOperation)
        {
            var tasks = Batch(batchSize, items, asyncOperation);
            return await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<TOut>> BatchManyAsync<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, Task<IEnumerable<TOut>>> asyncOperation)
        {
            var tasks = Batch(batchSize, items, asyncOperation);
            var results = await Task.WhenAll(tasks);
            return results.SelectMany(x => x);
        }

        public static IEnumerable<IEnumerable<T>> CreateBatches<T>(IEnumerable<T> list, int batchSize)
        {
            int i = 0;
            return list.GroupBy(x => (i++ / batchSize)).ToList();
        }
    }
}
