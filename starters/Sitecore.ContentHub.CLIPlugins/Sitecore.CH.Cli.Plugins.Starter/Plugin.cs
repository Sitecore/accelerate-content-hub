using Microsoft.Extensions.DependencyInjection;
using Sitecore.CH.Cli.Core.Abstractions.Infrastructure;
using Sitecore.CH.Cli.Core.Extensions;
using Sitecore.CH.Cli.Plugins.Starter.CommandHandlers;
using Sitecore.CH.Cli.Plugins.Starter.Commands;
using Sitecore.CH.Cli.Plugins.Starter.Model.Parameters;
using Sitecore.ContentHub.CLIPlugins.Base;

namespace Sitecore.ContentHub.CLIPlugins.Starter
{
    public class Plugin : IPlugin
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCommandHandler<HelloCommandHandler>()
                .AddCommandHandler<EchoCommandHandler, EchoParameters>()
                .AddCommandHandler<GetEntityCommandHandler, GetEntityParameters>()

                .AddBaseServices();
        }

        public void RegisterCommands(ICommandRegistry registry)
        {
            registry.RegisterCommandGroup("starter", [new HelloCommand(), new EchoCommand(), new GetEntityCommand()], "CLI plugin starter test commands");
        }
    }
}
