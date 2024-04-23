namespace PsimCsLib.Models;

public sealed class PrivateSystemMessage
{
    public string Message { get; }

    public PrivateSystemMessage(string message)
    {
        Message = message;
    }
}