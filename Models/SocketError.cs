namespace PsimCsLib.Models;

public sealed class SocketError
{
	public Exception Exception { get; }

	internal SocketError(Exception exception)
	{
		Exception = exception;
	}
}