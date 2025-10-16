using FluentValidation;
using Mediator.Abstractions;

namespace Mediator.Extensions.FluentValidation;

/// <summary>
/// Validation behaviour for requests using FluentValidation.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehaviour<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        ArgumentNullException.ThrowIfNull(validators);

        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        // If no validators are registered, just continue and call the next handler.
        if (!_validators.Any())
        {
            return await next();
        }

        // Create a validation context.
        var context = new ValidationContext<TRequest>(request);

        // Run all validators and collect the results.
        var validationResults =
            await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        // If any validation failures, throw ValidationException.
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}