using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Rewiews.Api.Middlewares;
using Rewiews.Application;
using Rewiews.Infrastructure;
using Rewiews.Infrastructure.Context;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Reviews API", Version = "v1" });
});

builder.Services.AddHealthChecks()
    .AddCheck<MongoDbHealthCheck>("mongodb");

// 🔹 Application & Infrastructure DI
builder.AddApplicationServices(); // Application: MediatR, AutoMapper, FluentValidation, Behaviors
builder.Services.AddInfrastructure(builder.Configuration); // Infrastructure: Repos, MongoDB, IdGenerator

var app = builder.Build();

app.UseServiceDefaults();

using (var scope = app.Services.CreateScope())
{
    var seedManager = scope.ServiceProvider.GetRequiredService<SeedManager>();
    await seedManager.SeedAllAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Reviews API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();