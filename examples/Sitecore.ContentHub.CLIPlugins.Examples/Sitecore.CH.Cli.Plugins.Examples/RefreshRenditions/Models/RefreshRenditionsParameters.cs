namespace Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions.Models
{
    internal class RefreshRenditionsParameters
    {
        public long[]? AssetIds { get; set; }

        public string? FilePath { get; set; }

        public string[] RenditionNames { get; set; } = [];
    }
}
