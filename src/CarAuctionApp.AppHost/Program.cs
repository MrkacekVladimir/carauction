var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("carauctionapp-postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgAdmin();

var api = builder.AddProject<Projects.CarAuctionApp_WebApi>("carauctionapp-webapi")
    .WithReference(postgres)
    .WaitFor(postgres);

var frontend = builder.AddProject<Projects.CarAuctionApp_WebBlazor>("carauctionapp-blazor")
    .WithReference(api)
    .WaitFor(api);


builder.Build().Run();

