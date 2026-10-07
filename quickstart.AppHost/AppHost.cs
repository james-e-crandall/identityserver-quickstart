var builder = DistributedApplication.CreateBuilder(args);

var identityServer = 
    builder.AddProject<Projects.IdentityServer>("identityserver")
    .WithExternalHttpEndpoints(); // Ensures predictable host/port matching
    //.WithUrl("/.well-known/openid-configuration", "well-known openid-configuration");

var api = 
    builder.AddProject<Projects.Api>("api")
    .WithReference(identityServer);


var myworkerapp = builder.AddProject<Projects.Client>("myworkerapp")
    .WithReference(identityServer)
    .WithReference(api);

builder.Build().Run();
