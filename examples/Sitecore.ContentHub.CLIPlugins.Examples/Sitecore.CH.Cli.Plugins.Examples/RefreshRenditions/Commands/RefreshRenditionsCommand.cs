using Sitecore.CH.Cli.Core.Abstractions.Commands;
using Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions.CommandHandlers;

namespace Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions.Commands
{
    internal class RefreshRenditionsCommand : BaseCommand<RefreshRenditionsCommandHandler>
    {
        public RefreshRenditionsCommand() : base("generaterenditions", "Generate or refresh the given rendition for the provided asset ids")
        {
            AddOption<long[]>("The id of the asset to generate renditions for (can be used multiple times)", false, ["--assetids", "--assetid", "--id", "-i"]);
            AddOption<string>("The path of a text file containing asset ids (one per line) to generate renditions for", false, ["--filepath", "--file", "-f"]);
            AddOption<string[]>("The name of the rendition (can be used multiple times)", true, ["--renditionnames", "--renditionname", "--rendition", "-r"]);
        }
    }
}
