namespace Sitecore.CH.Cli.Plugins.Base.Constants
{
    public static class OptionListConstants
    {
        public static class JobConditions
        {
            public static string Pending = "Pending";
            public static string Success = "Success";
            public static string Failure = "Failure";
        }

        public static class JobStates
        {
            public static string Completed = "Completed";
            public static string Cancelled = "Cancelled";
            public static string InProgress = "InProgress";
            public static string Pending = "Pending";
            public static string Created = "Created";
            public static string PostCompleted = "Post_Completed";
            public static string PostInProgress = "Post_InProgress";
            public static string PostPending = "Post_Pending";
            public static string PostWaiting = "Post_Waiting";
            public static string PreCompleted = "Pre_Completed";
            public static string PreInProgress = "Pre_InProgress";
            public static string PrePending = "Pre_Pending";
            public static string PreWaiting = "Pre_Waiting";
            public static string ProcessingCompleted = "Processing_Completed";
            public static string ProcessingInProgress = "Processing_InProgress";
            public static string ProcessingPending = "Processing_Pending";
            public static string ProcessingWaiting = "Processing_Waiting";
            public static string Processing_Created = "Processing_Created";
        }

        public static class JobTypes
        {
            public static string Distribution = "Distribution";
            public static string PrintEntityGeneration = "PrintEntityGeneration";
            public static string Fetch = "Fetch";
            public static string Import = "Import";
            public static string MassEdit = "MassEdit";
            public static string Upload = "Upload";
            public static string Processing = "Processing";
            public static string Checkout = "Checkout";
            public static string Checkin = "Checkin";
            public static string Discard = "Discard";
            public static string Download = "Download";
            public static string Custom = "Custom";
            public static string Publishing = "Publishing";
            public static string System = "System";
            public static string Maintenance = "Maintenance";
            public static string Migration = "Migration";
            public static string Command = "Command";
            public static string MachineLearning = "MachineLearning";
            public static string EmailJob = "EmailJob";
            public static string ContentPublishing = "ContentPublishing";
        }
    }
}
