using PsimCsLib.Entities;

namespace PsimCsLib.Models;
public sealed class ClientJoinRoom
{
    public Room Room { get; }

    public ClientJoinRoom(Room room)
    {
        Room = room;
    }
}