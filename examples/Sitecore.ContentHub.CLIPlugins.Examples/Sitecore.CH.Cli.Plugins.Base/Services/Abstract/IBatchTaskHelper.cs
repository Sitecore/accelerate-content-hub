namespace Sitecore.CH.Cli.Plugins.Base.Services.Abstract
{
    public interface IBatchTaskHelper
    {
        public IEnumerable<TOut> Batch<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, TOut> operation);

        public IEnumerable<TOut> BatchMany<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, IEnumerable<TOut>> operation);

        Task BatchAsync<TItem>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, Task> asyncOperation);

        Task<IEnumerable<TOut>> BatchAsync<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, Task<TOut>> asyncOperation);

        public Task<IEnumerable<TOut>> BatchManyAsync<TItem, TOut>(int batchSize, IEnumerable<TItem> items, Func<IEnumerable<TItem>, Task<IEnumerable<TOut>>> asyncOperation);
    }
}
