using Microsoft.Extensions.Logging;
using Sitecore.CH.Cli.Plugins.Base.Services.Abstract;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
using Stylelabs.M.Sdk.Contracts.Querying;

namespace Sitecore.CH.Cli.Plugins.Base.Services.Concrete
{
    internal class EntityHelper : IEntityHelper
    {
        private readonly ILogger<EntityHelper> logger;
        private readonly IClientHelper clientHelper;
        private readonly IIteratorHelper iteratorHelper;

        public EntityHelper(ILogger<EntityHelper> logger, IClientHelper clientHelper, IIteratorHelper iteratorHelper)
        {
            this.logger = logger;
            this.clientHelper = clientHelper;
            this.iteratorHelper = iteratorHelper;
        }

        public async Task<IEntity> Create(string entityDefinitionName)
        {
            return await clientHelper.Execute(client => client.EntityFactory.CreateAsync(entityDefinitionName));
        }

        public async Task<IEntity> Get(long id, IEntityLoadConfiguration? loadConfig = null)
        {
            return await clientHelper.Execute(client => client.Entities.GetAsync(id, loadConfig));
        }

        public async Task<IEntity> Get(string identifier, IEntityLoadConfiguration? loadConfig = null)
        {
            return await clientHelper.Execute(client => client.Entities.GetAsync(identifier, loadConfig));
        }

        public async Task<long> Save(IEntity entity)
        {
            return await clientHelper.Execute(client => client.Entities.SaveAsync(entity));
        }

        public async Task Delete(IEntity entity)
        {
            if (!entity.Id.HasValue)
                return;
            await clientHelper.Execute(client => client.Entities.DeleteAsync(entity.Id.Value));
        }

        public async Task Delete(long entityId)
        {
            await clientHelper.Execute(client => client.Entities.DeleteAsync(entityId));
        }

        public IAsyncEnumerable<IEntity> GetByDefinition(string entityDefinitionName, IEntityLoadConfiguration? loadConfig = null)
        {
            logger.LogDebug($"Getting entities for definition {entityDefinitionName}");
            var entityIterator = clientHelper.ExecuteSync(client => client.Entities.GetEntityIterator(entityDefinitionName, loadConfig));
            return iteratorHelper.IterateAsync<IEntityQueryResult, IEntity>(entityIterator);
        }

        public IAsyncEnumerable<long> GetIdsByDefinition(string entityDefinitionName, IEntityLoadConfiguration? loadConfig = null)
        {
            logger.LogDebug($"Getting entity ids for definition {entityDefinitionName}");
            var entityIterator = clientHelper.ExecuteSync(client => client.Entities.GetEntityIdIterator(entityDefinitionName));
            return iteratorHelper.IterateAsync<IIdQueryResult, long>(entityIterator);
        }

        public async Task<long> GetIdFromIdentifierAsync(string identifier)
        {
            var entity = await clientHelper.ExecuteSync(client => client.Entities.GetAsync(identifier, EntityLoadConfiguration.Minimal));
            if (entity == null || !entity.Id.HasValue)
                throw new Exception($"Could not find entity with identifier {identifier}");
            return entity.Id.Value;
        }
    }
}
