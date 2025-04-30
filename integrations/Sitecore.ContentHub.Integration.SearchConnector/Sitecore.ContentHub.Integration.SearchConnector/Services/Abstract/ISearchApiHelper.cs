namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract
{
    public interface ISearchApiHelper
    {
        Task<string?> UpsertDocument(string entityId, string documentId, string locale, IDictionary<string, object> fields);

        Task<IEnumerable<string>?> DeleteDocument(string entityId, string documentId, string locale = "all");
    }
}
