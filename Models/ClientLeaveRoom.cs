using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class ClientLeaveRoom
{
    public Room Room { get; }

    internal ClientLeaveRoom(Room room)
    {
        Room = room;
    }
}