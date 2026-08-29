using Aspire.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);


var webControllerApi = builder.AddProject<Projects.WebControllerApi>("web-controller-api");
var webMinimalApi = builder.AddProject<Projects.WebMinimalApiAOT>("web-minimap-api");
var webMinimalApi = builder.AddProject<Projects.WebMinimalApiAOT>("web-minimap-api");
//var pgsql = builder.AddContainer("postgres", "postgres:latest")
//    .WithContainerName("binh-dev-pgsql-aspire")
//    .WithEnvironment("POSTGRES_PASSWORD", "170116Abc")
//    .WithEnvironment("POSTGRES_USER", "admin")
//    .WithEndpoint(5432, 5432);


//webControllerApi.WithReference(pgsql);


webControllerApi.PublishAsDockerFile();
webMinimalApi.PublishAsDockerFile();


var webServerApi = builder.AddProject<Projects.FunctionApp>("function-app");
builder.AddProject<Projects.RateLimiterService>("ratelimiterservice");
builder.Build().Run();
