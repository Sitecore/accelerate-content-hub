using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sitecore.CH.Integration.AssetImporter.Functions.Services.Abstract;
using Sitecore.CH.Integration.AssetImporter.Functions.Services.Concrete;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication().AddAzureBlobClient("asset-import");

builder.Services
    .AddTransient<IApplicationSettings, ApplicationSettings>()
    .AddTransient<IContentHubClientFactory, ContentHubClientFactory>()
    .AddTransient<IContentHubClientHelper, ContentHubClientHelper>()
    .AddTransient<IStorageService, StorageService>()
    .AddTransient<IUploadService, UploadService>()
    .AddTransient<IUploadWorker, UploadWorker>();

builder.Build().Run();
