var builder = DistributedApplication.CreateBuilder(args);

var storage = builder
    .AddAzureStorage("storage");

// todo: make configurable    
storage = storage.RunAsEmulator();

var blobs = storage.AddBlobs("asset-import");

builder
    .AddAzureFunctionsProject<Projects.Sitecore_CH_Integration_AssetImporter_Functions>("functions")
    .WithHostStorage(storage)
    .WithReference(blobs)
    .WaitFor(blobs);

builder.Build().Run();
