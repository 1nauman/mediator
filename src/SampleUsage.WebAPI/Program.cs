using System.Reflection;
using Mediator;
using SampleUsage.WebAPI.Application.Weather.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediator(Assembly.GetExecutingAssembly());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async (IMediator mediator) =>
    {
        var query = new GetWeatherForecastsQuery();
        var forecasts = await mediator.SendAsync(query);

        return forecasts
            .Select(o => new WeatherForecast(o.Date, o.TemperatureC, o.Summary, o.TemperatureF))
            .ToArray();
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, int TemperatureF);