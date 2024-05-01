using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class PrivateMessage
{
	public PsimUsername Sender { get; }
    public string Message { get; }
    public bool IsIntro { get; }

    internal PrivateMessage(PsimUsername sender, string message, bool isIntro)
    {
        Sender = sender;
        Message = message;
        IsIntro = isIntro;
    }
}