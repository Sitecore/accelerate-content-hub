var builder = DistributedApplication.CreateBuilder(args);

var storage = builder
    .AddAzureStorage("storage");

// todo: make configurable    
storage = storage.RunAsEmulator();

var blobs = storage.AddBlobs("asset-import");

builder.AddAzureFunctionsProject<Projects.Sitecore_ContentHub_Integration_AssetImporter>("sitecore-contenthub-integration-assetimporter")
    .WithHostStorage(storage)
    .WithReference(blobs)
    .WaitFor(blobs);

builder.Build().Run();
