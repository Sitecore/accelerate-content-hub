using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sitecore.ContentHub.Integration.OrderCloudConnector.Models.Options;
using Sitecore.ContentHub.Integration.OrderCloudConnector.Services.Abstract;
using Sitecore.ContentHub.Integration.OrderCloudConnector.Services.Concrete;

var builder = FunctionsApplication.CreateBuilder(args);

var queueName = builder.Configuration.GetValue<string>($"{ServiceBusOptions.ConfigurationSectionName}:{nameof(ServiceBusOptions.Name)}") ?? "";
builder.AddServiceDefaults()
    .AddAzureServiceBusClient(queueName);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddTransient<IMessageWorker, MessageWorker>();

builder.Services
    .AddOptions<ServiceBusOptions>()
    .BindConfiguration(ServiceBusOptions.ConfigurationSectionName)
    .ValidateDataAnnotations();

builder.Build().Run();
