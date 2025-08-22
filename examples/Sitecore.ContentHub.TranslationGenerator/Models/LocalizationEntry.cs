using System.Globalization;

namespace Sitecore.CH.TranslationGenerator.Models
{
    public class LocalizationEntry
    {
        public long Id { get; set; }
        public required string Identifier { get; set; }
        public required string EntryName { get; set; }
        public required string BaseTemplate { get; set; }
        public Dictionary<CultureInfo, string> Templates { get; set; } = [];
    }
}
