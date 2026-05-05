using Api.Middleware;
using Application;
using CoreMesh.Endpoints.Extensions;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpoints([typeof(Program).Assembly]);
builder.Services.AddExceptionHandler<BlogExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapOpenApi();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();
