namespace PsimCsLib;

public static class Extensions
{
    public static Task Raise<TEventArgs>(this Func<TEventArgs, Task> handlers, TEventArgs args)
        where TEventArgs : EventArgs
    {
        if (handlers != null)
        {
            return Task.WhenAll(handlers.GetInvocationList()
                .OfType<Func<TEventArgs, Task>>()
                .Select(h => h(args)));
        }

        return Task.CompletedTask;
    }
}