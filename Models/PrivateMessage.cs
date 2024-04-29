using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class PrivateMessage
{
    public User Sender { get; }
    public string Message { get; }
    public bool IsIntro { get; }

    internal PrivateMessage(User sender, string message, bool isIntro)
    {
        Sender = sender;
        Message = message;
        IsIntro = isIntro;
    }
}