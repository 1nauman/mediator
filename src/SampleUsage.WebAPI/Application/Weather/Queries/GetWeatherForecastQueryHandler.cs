using Mediator;

namespace SampleUsage.WebAPI.Application.Weather.Queries;

public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastsQuery, WeatherForecast[]>
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public async Task<WeatherForecast[]> HandleAsync(GetWeatherForecastsQuery request,
        CancellationToken cancellationToken = default)
    {
        var forecasts = Enumerable.Range(1, 10)
            .Select(index => new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ));

        await Task.Delay(100, cancellationToken);

        return forecasts.ToArray();
    }
}