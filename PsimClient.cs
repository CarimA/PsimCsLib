using PsimCsLib.Entities;
using PsimCsLib.Models;
using PsimCsLib.Modules;
using PsimCsLib.PubSub;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace PsimCsLib;

public sealed class PsimClient : Publisher
{
    public WebSocketState State => _socket.State;
    public bool LoggedIn { get; internal set; }

    public RoomCollection Rooms { get; }
    internal UserCollection Users { get; }
    public PsimClientOptions Options { get; }

    private readonly ClientWebSocket _socket;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ConcurrentQueue<(string Message, TaskCompletionSource Task)> _messageQueue;
    private string _closeDescription;

    public PsimClient(PsimClientOptions options)
    {
        Options = options;
        _socket = new ClientWebSocket();
        _cancellationTokenSource = new CancellationTokenSource();
        _messageQueue = new ConcurrentQueue<(string, TaskCompletionSource)>();
        _closeDescription = string.Empty;

        Rooms = new RoomCollection(this);
        Users = new UserCollection(this);

        Subscribe(new ProcessByteBuffer(this));
        Subscribe(new ProcessCommands(this));
        Subscribe(new Authentication(this));
    }

    public async Task Connect(bool autoReconnect = false)
    {
        var firstRun = true;
        while (autoReconnect || firstRun)
        {
            firstRun = false;

            try
            {
                await _socket.ConnectAsync(new Uri(Options.ToServerUri()), _cancellationTokenSource.Token);
                await Publish(new SocketConnected());

                await Task.WhenAny(Send(), Receive(), CheckDisconnect());
            }
            catch (WebSocketException ex)
            {
                await Publish(new SocketError(ex));

                if (IsUnrecoverableWebsocketError(ex.WebSocketErrorCode))
                    return;
            }
            finally
            {
                await Cleanup();
            }

            await Task.Delay(500);
        }
    }

    private bool IsUnrecoverableWebsocketError(WebSocketError error)
    {
        return error is WebSocketError.NotAWebSocket or WebSocketError.UnsupportedProtocol
            or WebSocketError.UnsupportedVersion;
    }

    private async Task Cleanup()
    {
        var status = _socket.CloseStatus ?? WebSocketCloseStatus.NormalClosure;
        var desc = _socket.CloseStatusDescription ?? _closeDescription;
        await Publish(new SocketDisconnected(status, desc));
        _socket?.Dispose();
    }

    public void Disconnect(string reason)
    {
        _closeDescription = reason;
        _cancellationTokenSource.Cancel();
    }

    private async Task CheckDisconnect()
    {
        var token = _cancellationTokenSource.Token;

        while (_socket.State == WebSocketState.Open)
        {
            if (token.IsCancellationRequested)
            {
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
                return;
            }

            await Task.Delay(50);
        }
    }

    internal async Task Send(string message)
    {
        var tcs = new TaskCompletionSource();
        _messageQueue.Enqueue((message, tcs));
        await tcs.Task;
    }

    private async Task Send()
    {
        while (_socket.State == WebSocketState.Open)
        {
            if (!LoggedIn)
            {
                await Task.Delay(50);
                continue;
            }

            if (_messageQueue.TryDequeue(out var item))
            {
                await ForceSend(item.Message);
                item.Task.SetResult();
            }

            await Task.Delay(500);
        }
    }

    internal async Task ForceSend(string message)
    {
        var buffer = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(message));
        await _socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task Receive()
    {
        var buffer = new Memory<byte>(new byte[256]);
        await using var dataStream = new MemoryStream();

        while (_socket.State == WebSocketState.Open)
        {
            ValueWebSocketReceiveResult result;

            do
            {
                result = await _socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Disconnect("Disconnect requested by server.");
                    return;
                }

                await dataStream.WriteAsync(buffer[..result.Count], CancellationToken.None);
            } while (!result.EndOfMessage);

            var bytes = dataStream.ToArray();
            await Publish(new ByteBuffer(bytes));

            dataStream.SetLength(0);
        }
    }

    public async Task SetAvatar(string avatar)
    {
        await Send($"|/avatar {avatar}");
    }

    public async Task SetStatus(string status)
    {
        await Send($"|/status ${status}");
    }
}