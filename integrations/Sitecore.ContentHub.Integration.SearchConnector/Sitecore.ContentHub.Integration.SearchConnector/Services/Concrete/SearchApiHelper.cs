using Microsoft.Extensions.Options;
using Sitecore.ContentHub.Integration.SearchConnector.Models.Options;
using Sitecore.ContentHub.Integration.SearchConnector.Models.Search;
using Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract;
using System.Net.Http.Json;

namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Concrete
{
    class SearchApiHelper(HttpClient httpClient, IOptions<SearchOptions> searchOptions) : ISearchApiHelper
    {
        public async Task<string?> UpsertDocument(string entityId, string documentId, string locale, IDictionary<string, object> fields)
        {
            var documentRequest = new DocumentRequestMessage
            {
                Document = new Document
                {
                    Id = documentId,
                    Fields = fields
                }
            };
            var response = await httpClient.PutAsJsonAsync($"ingestion/v1/domains/{searchOptions.Value.DomainId}/sources/{searchOptions.Value.SourceId}/entities/{entityId}/documents/{documentId}?locale={locale}", documentRequest);
            response.EnsureSuccessStatusCode();
            var responseMessage = await response.Content.ReadFromJsonAsync<DocumentResponseMessage>();
            return responseMessage?.IncrementalUpdateId;
        }

        public async Task<IEnumerable<string>?> DeleteDocument(string entityId, string documentId, string locale = "all")
        {
            var response = await httpClient.DeleteAsync($"ingestion/v1/domains/{searchOptions.Value.DomainId}/sources/{searchOptions.Value.SourceId}/entities/{entityId}/documents/{documentId}?locale={locale}");
            response.EnsureSuccessStatusCode();
            var responseMessage = await response.Content.ReadFromJsonAsync<DeleteDocumentResponseMessage>();
            return responseMessage?.IncrementalUpdateIds;
        }
    }
}
