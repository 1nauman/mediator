using FluentValidation;

namespace SampleUsage.WebAPI.Application.Weather.Queries;

public class GetWeatherForecastsQueryValidator : AbstractValidator<GetWeatherForecastsQuery>
{
    public GetWeatherForecastsQueryValidator()
    {
        RuleFor(o => o.Location)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Location must be between 1 and 50 characters");
    }
}