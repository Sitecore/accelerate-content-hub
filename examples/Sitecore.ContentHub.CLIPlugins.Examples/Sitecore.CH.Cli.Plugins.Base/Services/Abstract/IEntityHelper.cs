using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Sdk.Contracts.Base;
namespace Sitecore.CH.Cli.Plugins.Base.Services.Abstract
{
    public interface IEntityHelper
    {
        Task<IEntity> Create(string entityDefinitionName);

        Task<IEntity> Get(long id, IEntityLoadConfiguration? loadConfig = null);

        Task<IEntity> Get(string identifier, IEntityLoadConfiguration? loadConfig = null);

        Task<long> Save(IEntity entity);

        Task Delete(IEntity entity);

        Task Delete(long entityId);


        IAsyncEnumerable<IEntity> GetByDefinition(string entityDefinitionName, IEntityLoadConfiguration? loadConfig = null);

        IAsyncEnumerable<long> GetIdsByDefinition(string entityDefinitionName, IEntityLoadConfiguration? loadConfig = null);

        Task<long> GetIdFromIdentifierAsync(string identifier);
    }
}
