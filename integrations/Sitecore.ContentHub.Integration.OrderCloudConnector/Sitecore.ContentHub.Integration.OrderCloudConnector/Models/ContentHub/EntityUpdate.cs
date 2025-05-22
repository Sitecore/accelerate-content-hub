using System.Text.Json.Serialization;

namespace Sitecore.ContentHub.Integration.OrderCloudConnector.Models.ContentHub
{
    public class EntityUpdate
    {
        public required string Identifier { get; set; }

        [JsonPropertyName("entity_definition")]
        public required string EntityDefinition { get; set; }
        public required string Operation { get; set; }
    }
}
