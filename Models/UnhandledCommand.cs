namespace PsimCsLib.Models;
public sealed class UnhandledCommand
{
    public PsimData Data { get; }

    public UnhandledCommand(PsimData data)
    {
        Data = data;
    }
}
