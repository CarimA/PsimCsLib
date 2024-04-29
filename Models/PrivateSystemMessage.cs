namespace PsimCsLib.Models;

public sealed class PrivateSystemMessage
{
    public string Message { get; }

    internal PrivateSystemMessage(string message)
    {
        Message = message;
    }
}