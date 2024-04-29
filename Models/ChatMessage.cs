using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class ChatMessage
{
    public Room Room { get; }
    public DateTime DatePosted { get; }
    public User User { get; }
    public string Message { get; }
    public bool IsIntro { get; }

    internal ChatMessage(Room room, DateTime datePosted, User user, string message, bool isIntro)
    {
        Room = room;
        DatePosted = datePosted;
        User = user;
        Message = message;
        IsIntro = isIntro;
    }
}