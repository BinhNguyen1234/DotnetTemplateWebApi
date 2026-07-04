using Microsoft.EntityFrameworkCore;
using WebControllerApi.Insfrastructure;
using WebControllerApi.Insfrastructure.Database.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TestContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database1")
));
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
