using Microsoft.Extensions.DependencyInjection;

namespace Base.Infrastructure.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    Task SendAsync<T>(T message, string destination = null, CancellationToken cancellationToken = default) where T : class;
    void Subscribe<T, THandler>() where T : class where THandler : IMessageHandler<T>;

    void Subscribe<T>();

    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}

public interface IMessageHandler<in T> where T : class
{
    Task HandleAsync(T message, CancellationToken cancellationToken = default);
}

public interface IMessageBusProvider
{
    void Configure(IServiceCollection services, string messagingType);
}

public class InMemoryMessageBus : IMessageBus
{
    private readonly List<object> _events = new();

    public Task PublishAsync(object @event)
    {
        _events.Add(@event);
        return Task.CompletedTask;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        throw new NotImplementedException();
    }

    public Task SendAsync<T>(T message, string destination = null, CancellationToken cancellationToken = default) where T : class
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Subscribe<T, THandler>()
        where T : class
        where THandler : IMessageHandler<T>
    {
        throw new NotImplementedException();
    }

    public void Subscribe<T>()
    {
        throw new NotImplementedException();
    }
}