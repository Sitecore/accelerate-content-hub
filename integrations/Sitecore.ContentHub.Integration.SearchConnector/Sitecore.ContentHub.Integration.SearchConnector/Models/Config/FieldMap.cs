using Sitecore.ContentHub.Integration.SearchConnector.Models.Config.FieldMaps;
using System.Text.Json.Serialization;

namespace Sitecore.ContentHub.Integration.SearchConnector.Models.Config
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(PropertyFieldMap), typeDiscriminator: "property")]
    [JsonDerivedType(typeof(RelationFieldMap), typeDiscriminator: "relation")]
    public class FieldMap
    {
        public required string SearchAttributeName { get; set; }

        //public required string Type { get; set; }
    }
}
