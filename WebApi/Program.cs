using Application.Extensions;
using Application.SignalRHub;
using Infrastructure.Extensions;
using Persistence.Context;
using Persistence.Extensions;
using WebApi.Extensions;

const string CORS_POLICY = "CorsPolicy";
const string DOCKER_ENVIRONMENT = "Docker";

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;

// Add services to the container.
builder.Services.AddPresentationLayer(builder.Configuration, CORS_POLICY);
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration, environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment() 
    || app.Environment.IsEnvironment(DOCKER_ENVIRONMENT))
{
    app.UseSwagger()
        .UseSwaggerUI()
        .UseExceptionHandler();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseCors(CORS_POLICY)
    .UseAuthentication()
    .UseAuthorization();

app.MapHub<ApplicationHub>("/hub");

app.MapControllers();

app.MigrateDatabase<ManageEmployeesContext>(builder.Configuration);

await app.RunAsync();