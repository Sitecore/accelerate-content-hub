using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Sitecore.ContentHub.Integration.SearchConnector.Models.Options;
using Sitecore.ContentHub.Integration.SearchConnector.Services.Abstract;
using Sitecore.ContentHub.Integration.SearchConnector.Services.Concrete;

var builder = FunctionsApplication.CreateBuilder(args);

var queueName = builder.Configuration.GetValue<string>($"{ServiceBusOptions.ConfigurationSectionName}:{nameof(ServiceBusOptions.Name)}") ?? "";
builder.AddServiceDefaults()
    .AddAzureServiceBusClient(queueName);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddSingleton<IConfigHelper, ConfigHelper>()
    .AddTransient<IContentHubClientFactory, ContentHubClientFactory>()
    .AddTransient<IContentHubClientHelper, ContentHubClientHelper>()
    .AddTransient<IContentHubEntityService, ContentHubEntityService>()
    .AddTransient<IMessageWorker, MessageWorker>();

builder.Services
    .AddOptions<ContentHubOptions>()
    //.Bind(builder.Configuration.GetSection(ContentHubOptions.ConfigurationSectionName))
    .BindConfiguration(ContentHubOptions.ConfigurationSectionName)
    .ValidateDataAnnotations();

builder.Services
    .AddOptions<SearchOptions>()
    //.Bind(builder.Configuration.GetSection(SearchOptions.ConfigurationSectionName))
    .BindConfiguration(SearchOptions.ConfigurationSectionName)
    .ValidateDataAnnotations();

builder.Services
    .AddOptions<ServiceBusOptions>()
    //.Bind(builder.Configuration.GetSection(ServiceBusOptions.ConfigurationSectionName))
    .BindConfiguration(ServiceBusOptions.ConfigurationSectionName)
    .ValidateDataAnnotations();

builder.Services
    .AddHttpClient<ISearchApiHelper, SearchApiHelper>((services, client) =>
    {
        var searchOptions = services.GetRequiredService<IOptions<SearchOptions>>().Value;

        client.BaseAddress = searchOptions.BaseUrl;
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, searchOptions.AuthToken);
    });

builder.Build().Run();
