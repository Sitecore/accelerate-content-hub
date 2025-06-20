﻿using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using Sitecore.CH.TranslationGenerator.Constants;
using Sitecore.CH.TranslationGenerator.Models;
using Sitecore.CH.TranslationGenerator.Services.Abstract;
using System.Globalization;

namespace Sitecore.CH.TranslationGenerator.Services.Concrete
{
    internal class ExcelService : IExcelService
    {
        public IEnumerable<LocalizationEntry> GetLocalizationEntries(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return stream.Query(true, SchemaConstants.LocalizationEntry.DefinitionName).Select(x => GetLocalizationEntryFromExcelRow((IDictionary<string, object>)x)).ToList();
        }

        public IEnumerable<PortalPage> GetPortalPages(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return stream.Query(true, SchemaConstants.PortalPage.DefinitionName).Select(x => GetPortalPageFromExcelRow((IDictionary<string, object>)x)).ToList();
        }

        public void Save(string filePath, IEnumerable<LocalizationEntry> localizationEntries, IEnumerable<PortalPage> portalPages)
        {
            using var stream = File.OpenWrite(filePath);
            var config = new OpenXmlConfiguration()
            {
                TableStyles = TableStyles.None
            };
            var data = new Dictionary<string, object>
            {
                { SchemaConstants.LocalizationEntry.DefinitionName, localizationEntries.Select(GetExcelRowFromLocalizationEntry) },
                { SchemaConstants.PortalPage.DefinitionName, portalPages.Select(GetExcelRowFromPortalPage) }
            };
            stream.SaveAs(data, true, configuration: config);
        }

        private static LocalizationEntry GetLocalizationEntryFromExcelRow(IDictionary<string, object> row)
        { 
            return new LocalizationEntry
            {
                Id = Convert.ToInt64((double)row[SchemaConstants.Shared.Properties.Id]),
                Identifier = (string)row[SchemaConstants.Shared.Properties.Identifier],
                EntryName = (string)row[SchemaConstants.LocalizationEntry.Properties.Name],
                BaseTemplate = (string)row[SchemaConstants.LocalizationEntry.Properties.BaseTemplate],
                Templates = row.Where(c => c.Key.StartsWith(SchemaConstants.LocalizationEntry.Properties.Template)).ToDictionary(c => GetCultureInfoFromColumnName(c.Key), c => (string)c.Value)
            };
        }

        private static PortalPage GetPortalPageFromExcelRow(IDictionary<string, object> row)
        {
            return new PortalPage
            {
                Name = (string)row[SchemaConstants.PortalPage.Properties.Name],
                Titles = row.Where(c => c.Key.StartsWith(SchemaConstants.PortalPage.Properties.Title)).ToDictionary(c => GetCultureInfoFromColumnName(c.Key), c => (string)c.Value)
            };
        }

        private IDictionary<string, object> GetExcelRowFromLocalizationEntry(LocalizationEntry localizationEntry)
        {
            var row = new Dictionary<string, object>()
            {
                { SchemaConstants.Shared.Properties.Id, localizationEntry.Id },
                { SchemaConstants.Shared.Properties.Identifier, localizationEntry.Identifier },
                { SchemaConstants.LocalizationEntry.Properties.Name, localizationEntry.EntryName },
                { SchemaConstants.LocalizationEntry.Properties.BaseTemplate, localizationEntry.BaseTemplate },
            };

            foreach(var template in localizationEntry.Templates)
                row.Add(GetColumnNameForCulture(SchemaConstants.LocalizationEntry.Properties.Template, template.Key), template.Value);

            return row;
        }

        private IDictionary<string, object> GetExcelRowFromPortalPage(PortalPage portalPage)
        {
            var row = new Dictionary<string, object>()
            {
                { SchemaConstants.PortalPage.Properties.Name, portalPage.Name },
            };

            foreach (var title in portalPage.Titles)
                row.Add(GetColumnNameForCulture(SchemaConstants.PortalPage.Properties.Title, title.Key), title.Value);

            return row;
        }

        private static CultureInfo GetCultureInfoFromColumnName(string columnName)
        {
            return new CultureInfo(columnName.Split(SchemaConstants.Shared.CultureSeparator).Last());
        }

        private static string GetColumnNameForCulture(string columnName, CultureInfo culture)
        {
            return $"{columnName}{SchemaConstants.Shared.CultureSeparator}{culture.Name}";
        }
    }
}
