using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class UserLeaveRoom
{
	public Room Room { get; }
	public PsimUsername User { get; }
	public bool IsIntro { get; }

	public UserLeaveRoom(Room room, PsimUsername user, bool isIntro)
	{
		Room = room;
		User = user;
		IsIntro = isIntro;
	}
}