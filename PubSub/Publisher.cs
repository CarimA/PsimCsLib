namespace PsimCsLib.PubSub;

public class Publisher
{
    protected readonly ISet<ISubscriber> _subscribers;

    public Publisher()
    {
        _subscribers = new HashSet<ISubscriber>();
    }

    internal async Task Publish<TEvent>(TEvent e)
    {
        var subscribers = new List<Task>();

        foreach (var subscriber in _subscribers)
            if (subscriber is ISubscriber<TEvent> es)
                subscribers.Add(es.HandleEvent(e));

        await Task.WhenAll(subscribers);
    }

    public bool Subscribe(ISubscriber subscriber)
    {
        return _subscribers.Add(subscriber);
    }

    public bool Unsubscribe(ISubscriber subscriber)
    {
        return _subscribers.Remove(subscriber);
    }
}

public class Publisher<TEvent>
{
    protected readonly ISet<ISubscriber<TEvent>> _subscribers;

    public Publisher()
    {
        _subscribers = new HashSet<ISubscriber<TEvent>>();
    }

    internal async Task Publish(TEvent e)
    {
        await Task.WhenAll(_subscribers.Select(subscriber => subscriber.HandleEvent(e)));
    }

    public bool Subscribe(ISubscriber<TEvent> subscriber)
    {
        return _subscribers.Add(subscriber);
    }

    public bool Unsubscribe(ISubscriber<TEvent> subscriber)
    {
        return _subscribers.Remove(subscriber);
    }
}