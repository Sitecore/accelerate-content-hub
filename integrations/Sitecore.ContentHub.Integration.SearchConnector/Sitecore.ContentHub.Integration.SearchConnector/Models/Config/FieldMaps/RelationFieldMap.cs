namespace Sitecore.ContentHub.Integration.SearchConnector.Models.Config.FieldMaps
{
    public class RelationFieldMap : FieldMap
    {
        public required string ContentHubRelationName { get; set; }

        public required string ContentHubRelatedPropertyName { get; set; }
    }
}
