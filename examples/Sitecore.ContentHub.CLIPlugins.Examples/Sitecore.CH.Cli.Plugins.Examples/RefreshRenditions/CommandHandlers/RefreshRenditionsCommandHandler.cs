using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sitecore.CH.Cli.Core.Abstractions.Commands;
using Sitecore.CH.Cli.Core.Abstractions.Rendering;
using Stylelabs.M.Sdk.WebClient;
using System.CommandLine.Invocation;
using Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions.Models;
using Sitecore.CH.Cli.Core.Extensions;
using Sitecore.CH.Cli.Core.Rendering;
using Newtonsoft.Json.Linq;
using Sitecore.CH.Cli.Plugins.Base.Services.Abstract;
using Stylelabs.M.Framework.Essentials.LoadConfigurations;
using Stylelabs.M.Framework.Essentials.LoadOptions;
using Stylelabs.M.Sdk.Contracts.Base;
using Sitecore.CH.Cli.Plugins.Base.Constants;

namespace Sitecore.CH.Cli.Plugins.Examples.RefreshRenditions.CommandHandlers
{
    class RefreshRenditionsCommandHandler : BaseCommandHandler
    {
        private readonly ILogger<RefreshRenditionsCommandHandler> logger;
        private readonly RefreshRenditionsParameters parameters;
        private readonly IBatchTaskHelper batchTaskHelper;
        private readonly IEntityHelper entityHelper;

        public RefreshRenditionsCommandHandler(ILogger<RefreshRenditionsCommandHandler> logger, Lazy<IWebMClient> client, IOutputRenderer renderer, IOptions<RefreshRenditionsParameters> parameters, IBatchTaskHelper batchTaskHelper, IEntityHelper entityHelper) : base(client, renderer)
        {
            this.logger = logger;
            this.parameters = parameters.Value;
            this.batchTaskHelper = batchTaskHelper;
            this.entityHelper = entityHelper;
        }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            try
            {
                if (parameters.AssetIds == null)
                {
                    if (string.IsNullOrEmpty(parameters.FilePath))
                        throw new ArgumentException("Either an asset id, or a file path must be provided");

                    var fileLines = await File.ReadAllLinesAsync(parameters.FilePath);
                    parameters.AssetIds = fileLines.Select(long.Parse).ToArray();
                }

                var jobIds = await Renderer.ShowLoadingIndicatorAsync("Creating Jobs", () => CreateRefreshRenditionsJobInBatchesAsync(parameters.AssetIds, parameters.RenditionNames));
                Renderer.RenderView(new ListView<long>(jobIds, view => view.AddColumn("Created Job Ids", id => id)));

                return 1;
            }
            catch (Exception ex)
            {
                Renderer.LogError("An exception occurred", ex);
                return 0;
            }
        }

        public async Task<IEnumerable<long>> CreateRefreshRenditionsJobInBatchesAsync(long[] targetIds, string[] renditionNames)
        {
            var batchSize = 2000;
            return await batchTaskHelper.BatchAsync(batchSize, targetIds, batchIds => CreateRefreshRenditionsJobAsync(batchIds.ToArray(), renditionNames));
        }

        public async Task<long> CreateRefreshRenditionsJobAsync(long[] targetIds, string[] renditionNames)
        {
            logger.LogInformation("Creating refresh renditions job");
            var refreshRenditionsOperation = TypedSerialiser(new
            {
                Renditions = renditionNames,
                FailedOnly = false,
                RefreshHistory = false,
                RefreshSubfiles = false
            }, "Stylelabs.M.Base.MassEdit.RefreshRenditionsOperation, Stylelabs.M.Base");


            var (jobId, _) = await CreateMassEditJobAsync(targetIds, new[] { refreshRenditionsOperation });

            return jobId;
        }

        private async Task<(long, IEntity)> CreateMassEditJobAsync(long[] targetIds, object[] operations)
        {
            var massEditJobConfiguration = TypedSerialiser(new
            {
                Operations = operations,
                FinalizeOperations = Array.Empty<string>(),
                Targets = targetIds
            }, "Stylelabs.M.Base.MassEdit.MassEditJobDescription, Stylelabs.M.Base");

            var jobId = await CreateJobAsync(OptionListConstants.JobTypes.MassEdit);
            await CreateJobDescriptionAsync(jobId, massEditJobConfiguration.ToString());

            var jobLoadConfiguration = new EntityLoadConfiguration
            {
                RelationLoadOption = RelationLoadOption.None,
                PropertyLoadOption = new PropertyLoadOption(SchemaConstants.Job.PropertyNames.State, SchemaConstants.Job.PropertyNames.TargetCount),
                CultureLoadOption = CultureLoadOption.None,
            };
            var jobEntity = await entityHelper.Get(jobId, jobLoadConfiguration);
            jobEntity.SetPropertyValue(SchemaConstants.Job.PropertyNames.State, OptionListConstants.JobStates.Pending);
            jobEntity.SetPropertyValue(SchemaConstants.Job.PropertyNames.TargetCount, Convert.ToInt64(targetIds.Length));
            await entityHelper.Save(jobEntity);

            return (jobId, jobEntity);
        }

        private async Task<long> CreateJobAsync(string jobType)
        {
            var jobEntity = await entityHelper.Create(SchemaConstants.Job.DefinitionName);
            jobEntity.SetPropertyValue(SchemaConstants.Job.PropertyNames.Condition, OptionListConstants.JobConditions.Pending);
            jobEntity.SetPropertyValue(SchemaConstants.Job.PropertyNames.Type, jobType);
            jobEntity.SetPropertyValue(SchemaConstants.Job.PropertyNames.State, OptionListConstants.JobStates.Created);
            jobEntity.SetPropertyValue(SchemaConstants.Job.PropertyNames.TargetCount, (long)0);
            jobEntity.SetPropertyValue(SchemaConstants.Job.PropertyNames.TargetsCompleted, (long)0);
            var jobId = await entityHelper.Save(jobEntity);
            logger.LogInformation($"Created job: {jobId}");
            return jobId;
        }

        private async Task<long> CreateJobDescriptionAsync(long jobId, string configuration)
        {
            var descriptionEntity = await entityHelper.Create(SchemaConstants.JobDescription.DefinitionName);
            descriptionEntity.SetPropertyValue(SchemaConstants.JobDescription.PropertyNames.Configuration, JToken.Parse(configuration));

            var jobRelation = descriptionEntity.GetRelation(SchemaConstants.JobDescription.RelationNames.JobToJobDescription, RelationRole.Child);
            jobRelation.SetIds(new long[] { jobId });
            var descriptionId = await entityHelper.Save(descriptionEntity);
            logger.LogInformation($"Created job description {descriptionId}");
            return descriptionId;
        }

        private JObject TypedSerialiser(object obj, string type)
        {
            var jobject = new JObject
            {
                { "$type", type }
            };

            foreach (var property in JObject.FromObject(obj))
            {
                jobject.Add(property.Key, property.Value);
            }
            return jobject;
        }
    }
}
