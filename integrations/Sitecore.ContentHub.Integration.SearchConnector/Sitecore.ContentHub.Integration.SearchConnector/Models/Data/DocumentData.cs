namespace Sitecore.ContentHub.Integration.SearchConnector.Models.Data
{
    public class DocumentData
    {
        public required string Culture { get; set; }

        public required IDictionary<string, object> Fields { get; set; }
    }
}
