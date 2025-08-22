namespace Sitecore.CH.Cli.Plugins.Base.Constants
{
    public class PlatformConstants
    {
        public class Client
        {
            public const int RequestRetryAttempts = 5;
            public const int RequestRetryDelayMin = 100;
            public const int RequestRetryDelayMax = 2000;
        }

        public class Query
        {
            public const int MaxTake = 1000;
            public const int MaxResultCount = 10000;
        }
    }
}
