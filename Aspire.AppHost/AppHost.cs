var builder = DistributedApplication.CreateBuilder(args);
builder
    .AddDockerfile("web-controller-api", "../WebControllerApi")
    .WithHttpEndpoint(port: 8080, targetPort: 8080);
builder.Build().Run();
