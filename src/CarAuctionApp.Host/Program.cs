var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("postgres-username", true);
var postgresPassword = builder.AddParameter("postgres-password", true);

var postgres = builder.AddPostgres("postgres", postgresUser, postgresPassword, port: 5432)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .AddDatabase("carauction");

var api = builder.AddProject<Projects.CarAuctionApp_WebApi>("api")
    .WithReference(postgres)
    .WaitFor(postgres);

var frontend = builder.AddProject<Projects.CarAuctionApp_WebBlazor>("blazor")
    .WithReference(api)
    .WaitFor(api);


builder.Build().Run();
