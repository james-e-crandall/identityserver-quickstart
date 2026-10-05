var builder = DistributedApplication.CreateBuilder(args);

var identityServer = 
    builder.AddProject<Projects.IdentityServer>("identityserver")
    .WithUrl("/.well-known/openid-configuration", "well-known openid-configuration");

builder.Build().Run();
