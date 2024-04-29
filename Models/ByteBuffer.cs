namespace PsimCsLib.Models;
public sealed class ByteBuffer
{
    public byte[] Buffer { get; }

    internal ByteBuffer(byte[] buffer)
    {
        Buffer = buffer;
    }
}