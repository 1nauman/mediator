namespace Mediator.Tests;

internal class TrackingBehaviour<TRequest, TResponse> : IPipelineBehaviour<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly List<string> _tracker;
    private readonly string _name;

    public TrackingBehaviour(List<string> tracker, string name)
    {
        _tracker = tracker;
        _name = name;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _tracker.Add($"Before-{_name}");
        var response = await next();
        _tracker.Add($"After-{_name}");
        return response;
    }
}