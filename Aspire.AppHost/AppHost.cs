using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);


var webControllerApi = builder.AddProject<Projects.WebControllerApi>("web-controller-api");
var webMinimalApi = builder.AddProject<Projects.WebMinimalApiAOT>("web-minimap-api");


webControllerApi.PublishAsDockerFile();
webMinimalApi.PublishAsDockerFile();


var webServerApi = builder.AddProject<Projects.FunctionApp>("function-app");
builder.Build().Run();
