namespace Sitecore.CH.Cli.Plugins.Base.Constants
{
    public static class SchemaConstants
    {
        public static class Component
        {
            public static class Identifiers
            {
                public const string External = "Portal.Component.External";
            }
        }

        public static class Job
        {
            public const string DefinitionName = "M.Job";

            public static class RelationNames
            {
                //public const string UserGroupToUser = "UserGroupToUser";
            }

            public static class PropertyNames
            {
                public const string Condition = "Job.Condition";
                public const string State = "Job.State";
                public const string TargetCount = "Job.TargetCount";
                public const string TargetsCompleted = "Job.TargetsCompleted";
                public const string Type = "Job.Type";
            }
        }

        public static class JobDescription
        {
            public const string DefinitionName = "M.JobDescription";

            public static class RelationNames
            {
                public const string JobToJobDescription = "JobToJobDescription";
            }

            public static class PropertyNames
            {
                public const string Configuration = "Job.Configuration";
            }
        }

        public static class PageComponent
        {
            public const string DefinitionName = "Portal.PageComponent";

            public static class RelationNames
            {
                public const string ComponentToPageComponent = "ComponentToPageComponent";
            }

            public static class PropertyNames
            {
                public const string Settings = "PageComponent.Settings";
            }
        }

        public static class UserGroup
        {
            public const string DefinitionName = "UserGroup";
        }
    }
}
