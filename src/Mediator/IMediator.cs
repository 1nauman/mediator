namespace Mediator;

/// <summary>
/// Represents a mediator that facilitates communication between different components in a system.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a request to the mediator and returns a response.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}