namespace Mediator;

/// <summary>
/// Represents a handler for processing requests.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified request and returns a response.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}