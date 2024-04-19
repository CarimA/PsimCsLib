namespace PsimCsLib.Models;
public sealed class RawData
{
    public byte[] Buffer { get; }

    public RawData(byte[] buffer)
    {
        Buffer = buffer;
    }
}
