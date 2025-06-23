# Sitecore.CH.TranslationGenerator
## Preparation
* Export languages from Content Hub (https://doc.sitecore.com/ch/en/users/content-hub/export-translations.html)
* Generate a key for the translation service https://learn.microsoft.com/en-us/azure/ai-services/translator/how-to/create-translator-resource
* Update the appsettings.local.json file with your API key.
 
## Application Usage
1. Build and run application.
2. You will be asked for a target language - enter e.g. 'de-DE' for German
3. Input file: enter the path to your language export file from the preparation steps.
4. Output file: you may choose your own file or just press enter again to select the provided default file, e.g. _inputfile (de-DE).xlsx_.
## Final Steps
* Repeat steps 2-4 for further translations.
* Import your new translations into Content Hub.
