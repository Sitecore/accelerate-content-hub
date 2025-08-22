using Microsoft.Extensions.DependencyInjection;
using Sitecore.CH.Cli.Core.Abstractions.Infrastructure;
using Sitecore.CH.Cli.Core.Commands.Endpoints;
using Sitecore.CH.Cli.Core.Extensions;
using Sitecore.CH.Cli.Plugins.Base;
using Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions.CommandHandlers;
using Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions.Models;

namespace Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions
{
    public class RefreshRenditionsPlugin : IPlugin
    {
            public void ConfigureServices(IServiceCollection services)
            {
                services
                    .AddCommandHandler<RefreshRenditionsCommandHandler, RefreshRenditionsParameters>()

                    .AddBaseServices();
            }

            public void RegisterCommands(ICommandRegistry registry)
            {
                registry.RegisterCommandGroup("renditions", [new RefreshEndpointCommand()], "CLI plugin refresh renditions example");
            }
        }
}
