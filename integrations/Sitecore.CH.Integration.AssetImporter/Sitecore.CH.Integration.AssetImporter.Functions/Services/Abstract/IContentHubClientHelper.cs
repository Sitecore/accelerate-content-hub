using Stylelabs.M.Sdk.WebClient;

namespace Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract
{
    public interface IContentHubClientHelper
    {
        Task Execute(Func<IWebMClient, Task> delegateFunc);

        Task<T> Execute<T>(Func<IWebMClient, Task<T>> delegateFunc);

        T ExecuteSync<T>(Func<IWebMClient, T> delegateFunc);
    }
}
