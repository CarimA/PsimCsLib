using PsimCsLib.Entities;

namespace PsimCsLib.Models;
public sealed class ClientJoinRoom
{
	public Room Room { get; }

	internal ClientJoinRoom(Room room)
	{
		Room = room;
	}
}