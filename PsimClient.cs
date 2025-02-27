using PsimCsLib.Entities;
using PsimCsLib.Models;
using PsimCsLib.Modules;
using PsimCsLib.PubSub;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using SocketError = PsimCsLib.Models.SocketError;

namespace PsimCsLib;

public class PsimClient : Publisher
{
	public WebSocketState State => _socket.State;
	public bool LoggedIn { get; internal set; }

	public RoomCollection Rooms { get; }
	public PsimClientOptions Options { get; }

	private ClientWebSocket _socket;
	private readonly CancellationTokenSource _cancellationTokenSource;
	private readonly ConcurrentQueue<(string Message, TaskCompletionSource Task)> _messageQueue;
	private string _closeDescription;

	private Dictionary<string, TaskCompletionSource<UserDetails>> _userDetailsRequests;

	public PsimClient(PsimClientOptions options)
	{
		Options = options;
		_cancellationTokenSource = new CancellationTokenSource();
		_messageQueue = new ConcurrentQueue<(string, TaskCompletionSource)>();
		_userDetailsRequests = new Dictionary<string, TaskCompletionSource<UserDetails>>();
		_closeDescription = string.Empty;

		Rooms = new RoomCollection(this);

		Subscribe(new ProcessByteBuffer(this));
		Subscribe(new ProcessCommands(this));
		Subscribe(new Authentication(this));
		Subscribe(new UserDetailsResolver(_userDetailsRequests));
		Subscribe(new ManageUsers(this));
	}

	public async Task Connect(bool reconnect)
	{
		if (reconnect)
		{
			while (true)
			{
				await Connect();
				await Task.Delay(500);
				Trace.WriteLine("[PsimCsLib] socket attempting to reconnect...");
			}
		}

		await Connect();
	}

	private async Task Connect()
	{
		try
		{
			_socket = new ClientWebSocket();
			_socket.Options.KeepAliveInterval = TimeSpan.FromMinutes(5);

			await _socket.ConnectAsync(new Uri(Options.ToServerUri()), _cancellationTokenSource.Token);
			Trace.WriteLine("[PsimCsLib] socket connected");
			await Publish(new SocketConnected());

			await Task.WhenAny(Send(), Receive(), CheckDisconnect());
		}
		catch (WebSocketException ex)
		{
			await Publish(new SocketError(ex));
			Trace.WriteLine($"[PsimCsLib] socket error: {ex}");
		}
		catch (SocketException ex)
		{
			await Publish(new SocketError(ex));
			Trace.WriteLine($"[PsimCsLib] socket error: {ex}");
		}
		finally
		{
			var status = _socket.CloseStatus ?? WebSocketCloseStatus.NormalClosure;
			var desc = _socket.CloseStatusDescription ?? _closeDescription;
			await Publish(new SocketDisconnected(status, desc));
			Trace.WriteLine($"[PsimCsLib] socket disconnected ({status}: {desc})");
			_socket?.Dispose();
		}
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
				Trace.WriteLine("[PsimCsLib] socket disconnect requested");
				await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
				return;
			}

			await Task.Yield();
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
				await Task.Delay(100);
				continue;
			}

			if (_messageQueue.TryDequeue(out var item))
			{
				await ForceSend(item.Message);
				item.Task.SetResult();
				await Task.Delay(200);
			}

			await Task.Yield();
		}
	}

	internal async Task ForceSend(string message)
	{
		var buffer = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(message));
		await _socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
	}

	private async Task Receive()
	{
		var token = _cancellationTokenSource.Token;
		var buffer = new Memory<byte>(new byte[1024]);
		await using var dataStream = new MemoryStream();

		while (_socket.State == WebSocketState.Open)
		{
			try
			{
				ValueWebSocketReceiveResult result;

				do
				{
					result = await _socket.ReceiveAsync(buffer, token);

					if (result.MessageType == WebSocketMessageType.Close)
					{
						Disconnect("Disconnect requested by server.");
						return;
					}

					await dataStream.WriteAsync(buffer[..result.Count], token);
				} while (!result.EndOfMessage);
			}
			catch (SocketException ex)
			{
				Trace.WriteLine($"[PsimCsLib] no packets received during keepalive: {ex}");
			}
			finally
			{
				var bytes = dataStream.ToArray();
				if (bytes.Length > 0)
					_ = Publish(new ByteBuffer(bytes)); // this is deliberate -- we do not want to block for Tasks called inside commands

				Debug.WriteLine($"[PsimCsLib] {Encoding.UTF8.GetString(bytes)}");

				dataStream.SetLength(0);
				await dataStream.FlushAsync();
			}

			await Task.Yield();
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

	public async Task<UserDetails?> GetUserDetails(string username, TimeSpan timeout)
	{
		var id = PsimUsername.TokeniseName(username);
		await Send($"|/cmd userdetails {id}");

		var tcs = new TaskCompletionSource<UserDetails>(TaskCreationOptions.RunContinuationsAsynchronously);
		_userDetailsRequests.Add(id, tcs);

		try
		{
			var result = await tcs.Task.WaitAsync(timeout);
			return result;
		}
		catch (TimeoutException)
		{
			tcs.SetCanceled();
			return null;
		}
		finally
		{
			_userDetailsRequests.Remove(id);
		}
	}
}