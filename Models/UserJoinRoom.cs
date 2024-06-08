using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class UserJoinRoom
{
    public Room Room { get; }
    public PsimUsername User { get; }
    public bool IsIntro { get; }

    public UserJoinRoom(Room room, PsimUsername user, bool isIntro)
    {
        Room = room;
        User = user;
        IsIntro = isIntro;
    }
}