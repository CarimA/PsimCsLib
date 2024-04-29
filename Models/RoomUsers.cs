using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class RoomUsers
{
    public Room Room { get; }
    public int Count { get; }
    public IReadOnlyList<(string RawName, User User)> Users { get; }

    public RoomUsers(Room room, int count, List<(string RawName, User User)> users)
    {
        Room = room;
        Count = count;
        Users = users.AsReadOnly();
    }
}
