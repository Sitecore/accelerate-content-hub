using Sitecore.CH.Cli.Core.Abstractions.Commands;
using Sitecore.CH.Cli.Plugins.Examples.Starter.CommandHandlers;

namespace Sitecore.CH.Cli.Plugins.Examples.Starter.Commands
{
    internal class GetEntityCommand : BaseCommand<GetEntityCommandHandler>
    {
        public GetEntityCommand() : base("get-entity", "A test command that get an entity by its id or identifier")
        {
            AddOption<long>("The ID of the entity to get", false, "--id", "-i");
            AddOption<string>("The identifier of the entity to get", false, "--identifier");
        }
    }
}
