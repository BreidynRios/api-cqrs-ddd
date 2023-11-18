using Application.Extensions;
using Infrastructure.Extensions;
using Persistence.Context;
using Persistence.Extensions;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseCustomExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<ManageEmployeesContext>(builder.Configuration);

app.Run();
