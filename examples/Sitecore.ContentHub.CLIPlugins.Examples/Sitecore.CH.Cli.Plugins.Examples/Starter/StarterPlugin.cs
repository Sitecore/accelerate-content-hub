using Microsoft.Extensions.DependencyInjection;
using Sitecore.CH.Cli.Core.Abstractions.Infrastructure;
using Sitecore.CH.Cli.Core.Extensions;
using Sitecore.CH.Cli.Plugins.Base;
using Sitecore.CH.Cli.Plugins.Examples.Starter.Commands;
using Sitecore.CH.Cli.Plugins.Examples.Starter.CommandHandlers;
using Sitecore.CH.Cli.Plugins.Examples.Starter.Model.Parameters;

namespace Sitecore.CH.Cli.Plugins.Examples.Starter
{
    public class StarterPlugin : IPlugin
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
