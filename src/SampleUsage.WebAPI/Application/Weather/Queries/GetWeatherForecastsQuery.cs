using Mediator;

namespace SampleUsage.WebAPI.Application.Weather.Queries;

public class GetWeatherForecastsQuery : IRequest<WeatherForecast[]>;