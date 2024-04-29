using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class RoomUsers
{
    public Room Room { get; }
    public int Count { get; }
    public IReadOnlyList<(RankPsimUsername, User)> Users { get; }

    internal RoomUsers(Room room, int count, List<(RankPsimUsername, User)> users)
    {
        Room = room;
        Count = count;
        Users = users.AsReadOnly();
    }
}
