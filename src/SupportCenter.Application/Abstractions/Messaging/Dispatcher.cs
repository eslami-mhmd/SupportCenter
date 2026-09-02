namespace SupportCenter.Application.Abstractions.Messaging;

public class Dispatcher : IDispatcher
{
    public Task<TResponse> Send<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }


    public Task<TResponse> Send<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}