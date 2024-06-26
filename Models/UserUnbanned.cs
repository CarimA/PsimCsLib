using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class UserUnbanned
{
	public UserUnbanned(PsimUsername user, Room room, PsimUsername staff, DateTime datePosted, bool isIntro)
	{
		User = user;
		Room = room;
		Staff = staff;
		DatePosted = datePosted;
		IsIntro = isIntro;
	}

	public PsimUsername User { get; }
	public Room Room { get; }
	public PsimUsername Staff { get; }
	public DateTime DatePosted { get; }
	public bool IsIntro { get; }
}