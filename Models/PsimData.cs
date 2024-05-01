using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class PsimData
{
	public Room Room { get; }
	public string Command { get; }
	public List<string> Arguments { get; }
	public bool IsIntro { get; }

	internal PsimData(Room room, string command, List<string> args, bool isIntro)
	{
		Room = room;
		Command = command;
		IsIntro = isIntro;
		Arguments = args;
	}
}