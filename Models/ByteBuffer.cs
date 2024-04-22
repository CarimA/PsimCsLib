namespace PsimCsLib.Models;
public sealed class ByteBuffer
{
    public byte[] Buffer { get; }

    public ByteBuffer(byte[] buffer)
    {
        Buffer = buffer;
    }
}