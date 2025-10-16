using Mediator;
using SampleUsage.WebAPI.Application.Weather.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediator();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/weatherforecast", async (IMediator mediator) =>
    {
        var query = new GetWeatherForecastsQuery();
        var forecasts = await mediator.SendAsync(query);

        var translatedForecasts = forecasts
            .Select(o => new WeatherForecast(o.Date, o.TemperatureC, o.TemperatureF, o.Summary))
            .ToArray();

        return translatedForecasts;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);