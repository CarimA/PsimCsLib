namespace PsimCsLib.Models;

public sealed class SocketError
{
    public Exception Exception { get; }

    public SocketError(Exception exception)
    {
        Exception = exception;
    }
}