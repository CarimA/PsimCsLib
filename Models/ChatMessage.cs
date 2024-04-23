using System.Xml.Linq;
using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class ChatMessage
{
    public Room Room { get; }
    public DateTime DatePosted { get; }
    public RoomUser User { get; }
    public string Message { get; }
    public bool IsIntro { get; }

    public ChatMessage(Room room, DateTime datePosted, RoomUser user, string message, bool isIntro)
    {
        Room = room;
        DatePosted = datePosted;
        User = user;
        Message = message;
        IsIntro = isIntro;
    }
}