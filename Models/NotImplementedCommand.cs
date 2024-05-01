namespace PsimCsLib.Models;
public sealed class NotImplementedCommand
{
	public PsimData Data { get; }

	internal NotImplementedCommand(PsimData data)
	{
		Data = data;
	}
}
