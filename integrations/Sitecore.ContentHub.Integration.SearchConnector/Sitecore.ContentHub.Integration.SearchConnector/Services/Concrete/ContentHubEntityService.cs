using Sitecore.ContentHub.Integration.SearchConnector.Constants;
using Sitecore.ContentHub.Integration.SearchConnector.Models.ContentHub;
using Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Sdk.Search;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace Sitecore.ContentHub.Integration.SearchConnector.Services.Concrete
{
    class ContentHubEntityService(IContentHubClientHelper clientHelper, IContentHubQueryBuilderService queryBuilderService, IContentHubQueryService queryService, IContentHubSearchService searchService) : IContentHubEntityService
    {
        public Task<IEntity> GetEntityById(long id, IEntityLoadConfiguration? loadConfiguration = null)
        {
            return clientHelper.Execute(client => client.Entities.GetAsync(id, loadConfiguration));
        }

        public Task<IEntity> GetEntityByIdentifier(string identifier, IEntityLoadConfiguration? loadConfiguration = null)
        {
            return clientHelper.Execute(client => client.Entities.GetAsync(identifier, loadConfiguration));
        }

        public IDictionary<string, object?> GetPropertyValueForCultures(IEntity entity, string propertyName, CultureInfo? fallbackCulture)
        {
            var isMultiLanguage = entity.GetProperty(propertyName).IsMultiLanguage;
            return entity.Cultures.ToDictionary(c => c.Name, c => isMultiLanguage ? GetPropertyValueForCulture(entity, propertyName, c, fallbackCulture) : entity.GetPropertyValue(propertyName));
        }

        public async Task<T?> GetRawEntityById<T>(long id)
        {
            var response = await clientHelper.Execute(client => client.Raw.GetAsync(GetEntityUrl(id)));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions() {  PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
        }

        public async Task<IEnumerable<IEntity>> GetRelatedEntities(IEnumerable<IEntity> entities, Relation relation, IEntityLoadConfiguration entityLoadConfiguration)
        {
            // todo: refactor
            var relationFilter = new RelationQueryFilter(relation.Name, relation.Role == RelationRole.Child ? Stylelabs.M.Framework.Essentials.LoadOptions.RelationRole.Child : Stylelabs.M.Framework.Essentials.LoadOptions.RelationRole.Parent);
            QueryNodeFilter? filter = null;
            foreach(var entityId in entities.Select(x => x.Id!.Value))
            {
                if (filter == null)
                    filter = relationFilter.Id == entityId;
                else
                    filter = new LogicalQueryFilter(filter, relationFilter.Id == entityId, LogicalOperator.Or);
            }

            return await searchService.SearchAfter(filter!, entityLoadConfiguration: entityLoadConfiguration);
        }

        public async Task<IEnumerable<IEntity>> GetRelatedEntities(IEntity entity, IEnumerable<Relation> relations, IEntityLoadConfiguration? entityLoadConfiguration = null)
        {
            if(!entity.Id.HasValue)
                return [];
            var entities = new List<IEntity> { entity }.AsEnumerable();
            return await GetRelatedEntities(entities, relations, entityLoadConfiguration);
        }

        public async Task<IEnumerable<IEntity>> GetRelatedEntities(IEnumerable<IEntity> entities, IEnumerable<Relation> relations, IEntityLoadConfiguration? entityLoadConfiguration = null)
        {
            foreach (var relation in relations)
                entities = await GetRelatedEntities(entities, relation, relations.Last() == relation ? entityLoadConfiguration ?? EntityLoadConfiguration.Default : EntityLoadConfiguration.Minimal);
            return entities;
        }

        private object? GetPropertyValueForCulture(IEntity entity, string propertyName, CultureInfo culture, CultureInfo? fallbackCulture)
        {
            return entity.GetPropertyValue(propertyName, culture) ?? (fallbackCulture != null ? entity.GetPropertyValue(propertyName, fallbackCulture) : null);
        }

        private string GetEntityUrl(long entityId)
        {
            return string.Format(ApiConstants.ContentHubClient.EntityUrlFormat, entityId);
        }
    }
}
