using Microsoft.Extensions.Logging;
using Sitecore.ContentHub.Integration.OrderCloudConnector.Models.ContentHub;
using Sitecore.ContentHub.Integration.OrderCloudConnector.Services.Abstract;

namespace Sitecore.ContentHub.Integration.OrderCloudConnector.Services.Concrete
{
    class MessageWorker(ILogger<MessageWorker> logger) : IMessageWorker
    {
        public Task ProcessUpdateMessage(UpdateMessage updateMessage)
        {
            foreach (var update in updateMessage.Updates)
            {
                logger.LogInformation($"Process update for entity {update.Identifier}, {update.EntityDefinition}, {update.Operation}");
            }
            return Task.CompletedTask;
        }
    }
}
