using Newtonsoft.Json.Linq;

namespace Sitecore.CH.Integration.AssetImporter.Functions.Models
{
    class UploadRequestResponse
    {
        public string Url { get; set; }

        public HttpContent Content { get; set; }
    }
}
