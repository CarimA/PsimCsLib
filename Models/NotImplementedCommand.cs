namespace PsimCsLib.Models;
public sealed class NotImplementedCommand
{
    public PsimData Data { get; }

    public NotImplementedCommand(PsimData data)
    {
        Data = data;
    }
}
