using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class UserRename
{
	public PsimUsername User { get; }
	public string OldId { get; }
	public bool IsIntro { get; }

	public UserRename(string oldId, PsimUsername user, bool isIntro)
	{
		OldId = oldId;
		User = user;
		IsIntro = isIntro;
	}
}