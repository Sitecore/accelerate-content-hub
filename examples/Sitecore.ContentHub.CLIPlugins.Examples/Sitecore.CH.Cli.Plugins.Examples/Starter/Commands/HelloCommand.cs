using Sitecore.CH.Cli.Core.Abstractions.Commands;
using Sitecore.CH.Cli.Plugins.Examples.Starter.CommandHandlers;

namespace Sitecore.CH.Cli.Plugins.Examples.Starter.Commands
{
    internal class HelloCommand : BaseCommand<HelloCommandHandler>
    {
        public HelloCommand() : base("hello", "A command for testing the plugin")
        {

        }
    }
}
