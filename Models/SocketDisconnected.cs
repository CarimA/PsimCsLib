using System.Net.WebSockets;

namespace PsimCsLib.Models;

public sealed class SocketDisconnected
{
    public WebSocketCloseStatus CloseStatus { get; }
    public string Reason { get; }

    internal SocketDisconnected(WebSocketCloseStatus closeStatus, string reason)
    {
        CloseStatus = closeStatus;
        Reason = reason;
    }
}