using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class UserRename
{
	public Room Room { get; }
	public PsimUsername User { get; }
	public string OldId { get; }
	public bool IsIntro { get; }

	public UserRename(Room room, string oldId, PsimUsername user, bool isIntro)
	{
		Room = room;
		OldId = oldId;
		User = user;
		IsIntro = isIntro;
	}
}