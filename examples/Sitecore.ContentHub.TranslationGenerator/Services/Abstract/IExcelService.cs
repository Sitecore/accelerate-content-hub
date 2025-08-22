using Sitecore.CH.TranslationGenerator.Models;

namespace Sitecore.CH.TranslationGenerator.Services.Abstract
{
    public interface IExcelService
    {
        IEnumerable<LocalizationEntry> GetLocalizationEntries(string filePath);

        IEnumerable<PortalPage> GetPortalPages(string filePath);

        void Save(string filePath, IEnumerable<LocalizationEntry> translationItems, IEnumerable<PortalPage> portalPages);
    }
}
