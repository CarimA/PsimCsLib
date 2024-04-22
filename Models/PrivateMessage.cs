namespace PsimCsLib.Models;

public sealed class PrivateMessage
{
    public string Recipient { get; }
    public string Sender { get; }
    public string Message { get; }
    public bool IsIntro { get; }
    
    public PrivateMessage(string recipient, string sender, string message, bool isIntro)
    {
        Recipient = recipient;
        Sender = sender;
        Message = message;
        IsIntro = isIntro;
    }
}