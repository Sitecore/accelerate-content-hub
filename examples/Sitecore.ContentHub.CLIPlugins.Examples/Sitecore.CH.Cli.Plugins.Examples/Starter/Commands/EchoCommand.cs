using Sitecore.CH.Cli.Core.Abstractions.Commands;
using Sitecore.CH.Cli.Plugins.Examples.Starter.CommandHandlers;

namespace Sitecore.CH.Cli.Plugins.Examples.Starter.Commands
{
    internal class EchoCommand : BaseCommand<EchoCommandHandler>
    {
        public EchoCommand() : base("echo", "A test command that will reply with the message you provide it")
        {
            AddOption<string>("The message to send", true, "--message", "-m");
        }
    }
}
