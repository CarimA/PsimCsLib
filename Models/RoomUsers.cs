using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class RoomUsers
{
	public Room Room { get; }
	public int Count { get; }
	public IReadOnlyList<PsimUsername> Users { get; }

	internal RoomUsers(Room room, int count, List<PsimUsername> users)
	{
		Room = room;
		Count = count;
		Users = users.AsReadOnly();
	}
}
