namespace PsimCsLib.Models;

public sealed class ChatMessage
{
    public DateTime DatePosted { get; }
    public string User { get; }
    public string Message { get; }
    public bool IsIntro { get; }

    public ChatMessage(DateTime datePosted, string user, string message, bool isIntro)
    {
        DatePosted = datePosted;
        User = user;
        Message = message;
        IsIntro = isIntro;
    }
}