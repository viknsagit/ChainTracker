var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ChainTracker>("chaintracker");

builder.AddProject<Projects.Blocks>("blocks");

builder.AddProject<Projects.Transactions>("transactions");

builder.AddProject<Projects.Contracts>("contracts");

builder.AddProject<Projects.Addresses>("addresses");

builder.AddProject<Projects.API>("api");

builder.AddProject<Projects.Tokens>("tokens");

builder.Build().Run();
