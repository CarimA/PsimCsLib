using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class ClientLeaveRoom
{
    public Room Room { get; }

    public ClientLeaveRoom(Room room)
    {
        Room = room;
    }
}