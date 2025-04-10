using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sitecore.ContentHub.Integration.AssetImporter.Services.Abstract;
using Sitecore.ContentHub.Integration.AssetImporter.Services.Concrete;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.ConfigureFunctionsWebApplication().AddAzureBlobClient("asset-import");

builder.Services
    .AddTransient<IApplicationSettings, ApplicationSettings>()
    .AddTransient<IContentHubClientFactory, ContentHubClientFactory>()
    .AddTransient<IContentHubClientHelper, ContentHubClientHelper>()
    .AddTransient<IStorageService, StorageService>()
    .AddTransient<IUploadService, UploadService>()
    .AddTransient<IUploadWorker, UploadWorker>();

builder.Build().Run();