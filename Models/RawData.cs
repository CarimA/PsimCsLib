using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed class RawData
{
	public Room Room { get; }
	public bool IsIntro { get; }
	public string Data { get; }

	internal RawData(Room room, bool isIntro, string data)
	{
		Room = room;
		IsIntro = isIntro;
		Data = data;
	}
}