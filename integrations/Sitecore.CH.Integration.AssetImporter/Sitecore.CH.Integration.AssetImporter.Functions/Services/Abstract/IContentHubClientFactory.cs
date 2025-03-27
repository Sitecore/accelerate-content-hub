using Stylelabs.M.Sdk.WebClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract
{
    interface IContentHubClientFactory
    {
        IWebMClient CreateClient();
    }
}
