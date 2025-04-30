using Microsoft.Extensions.Logging;
using Sitecore.ContentHub.Integration.SearchConnector.Models.Config;
using Sitecore.ContentHub.Integration.SearchConnector.Models.Config.FieldMaps;
using Sitecore.ContentHub.Integration.SearchConnector.Models.Data;
using Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Framework.Essentials.LoadOptions;
using Stylelabs.M.Sdk.Contracts.Base;
using System.Globalization;


namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Concrete
{
    class MessageWorker(ILogger<MessageWorker> logger, IContentHubEntityService entityService, ISearchApiHelper searchApiHelper, IConfigHelper configHelper) : IMessageWorker
    {
        public async Task<IEnumerable<string>?> DeleteSearchDocument(string contentHubEntityDefinition, string contentHubEntityIdentifier)
        {
            var definitionMap = GetDefinitionMap(contentHubEntityDefinition);
            return await searchApiHelper.DeleteDocument(definitionMap.SearchEntity, contentHubEntityIdentifier);
        }

        public async Task<IEnumerable<string>> UpsertSearchDocument(string contentHubEntityDefinition, string contentHubEntityIdentifier)
        {
            var definitionMap = GetDefinitionMap(contentHubEntityDefinition);

            // todo: need to produce an appropriate load configuration
            var entityLoadConfiguration = new EntityLoadConfigurationBuilder()
                .WithProperties(definitionMap.FieldMaps.Where(x => x is PropertyFieldMap).Cast<PropertyFieldMap>().Select(x => x.ContentHubPropertyName))
                .WithRelations(definitionMap.FieldMaps.Where(x => x is RelationFieldMap).Cast<RelationFieldMap>().Select(x => x.ContentHubRelationName))
                .InCultures(configHelper.CultureMaps.Select(m => new CultureInfo(m.ContentHubCulture)))
                .Build();

            var entity = await entityService.GetEntityByIdentifier(contentHubEntityIdentifier, entityLoadConfiguration) ?? throw new InvalidDataException($"Entity with identifier {contentHubEntityIdentifier} not found.");
            
            var cultureMaps = GetCultureMaps(entity.Cultures);

            var documentData = await GetDocumentData(cultureMaps, definitionMap, entity);

            var updateTasks = documentData.Select(dd => searchApiHelper.UpsertDocument(definitionMap.SearchEntity, contentHubEntityIdentifier, dd.Culture, dd.Fields));
            var incrementalUpdateIds = await Task.WhenAll(updateTasks);

            return incrementalUpdateIds.Where(x => x != null).Cast<string>();
        }

        private DefinitionMap GetDefinitionMap(string contentHubEntityDefinition)
        {
            var definitionMap = configHelper.DefinitionMaps.SingleOrDefault(m => m.ContentHubEntityDefinition == contentHubEntityDefinition);
            return definitionMap ?? throw new InvalidDataException($"Definition map for {contentHubEntityDefinition} not found.");
        }

        private IEnumerable<CultureMap> GetCultureMaps(IEnumerable<CultureInfo> contentHubCultures)
        {
            var contentHubCultureStrings = contentHubCultures.Select(c => c.Name).ToList();
            return configHelper.CultureMaps.Where(m => contentHubCultureStrings.Contains(m.ContentHubCulture));
        }

        private async Task<IEnumerable<DocumentData>> GetDocumentData(IEnumerable<CultureMap> cultureMaps, DefinitionMap definitionMap, IEntity entity)
        {
            // todo: refactor
            var documentDataDictionary = cultureMaps.ToDictionary(x => x.ContentHubCulture, x => new DocumentData { Culture = x.SearchCulture, Fields = new Dictionary<string, object>() });

            foreach (var fieldMap in definitionMap.FieldMaps)
            {
                if (fieldMap is PropertyFieldMap propertyFieldMap)
                {
                    entityService
                        .GetPropertyValueForCultures(entity, propertyFieldMap.ContentHubPropertyName)
                        .ToList()
                        .ForEach(x => documentDataDictionary[x.Key].Fields.Add(fieldMap.SearchAttributeName, x.Value));
                }
                else if (fieldMap is RelationFieldMap relationField)
                {
                    var relatedIds = entity.GetRelation(relationField.ContentHubRelationName).GetIds();
                    var relatedEntities = await Task.WhenAll(relatedIds.Select(id => entityService.GetEntityById(id)));
                    relatedEntities
                        .Select(x => entityService.GetPropertyValueForCultures(x, relationField.ContentHubRelatedPropertyName))
                        .SelectMany(d => d)
                        .GroupBy(x => x.Key, x => x.Value)
                        .ToList()
                        .ForEach(x => documentDataDictionary[x.Key].Fields.Add(fieldMap.SearchAttributeName, x.ToList()));
                }
            }
            return documentDataDictionary.Values;
        }
    }
}
