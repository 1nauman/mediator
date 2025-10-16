using Mediator.Abstractions;

namespace SampleUsage.WebAPI.Application.Weather.Queries;

public class GetWeatherForecastsQuery : IRequest<WeatherForecast[]>
{
    public string Location { get; set; } = string.Empty;
}