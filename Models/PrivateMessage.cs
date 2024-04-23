using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class PrivateMessage
{
    public RoomUser Sender { get; }
    public string Message { get; }
    public bool IsIntro { get; }
    
    public PrivateMessage(RoomUser sender, string message, bool isIntro)
    {
        Sender = sender;
        Message = message;
        IsIntro = isIntro;
    }
}