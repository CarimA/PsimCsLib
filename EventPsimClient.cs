using PsimCsLib.Models;
using PsimCsLib.PubSub;

namespace PsimCsLib;

public class EventPsimClient : PsimClient
{
	public event Func<ByteBuffer, Task> OnByteBuffer;
	public event Func<ChallengeString, Task> OnChallengeString;
	public event Func<ChatMessage, Task> OnChatMessage;
	public event Func<ClientJoinRoom, Task> OnClientJoinRoom;
	public event Func<ClientLeaveRoom, Task> OnClientLeaveRoom;
	public event Func<LoginFailure, Task> OnLoginFailure;
	public event Func<LoginSuccess, Task> OnLoginSuccess;
	public event Func<NotImplementedCommand, Task> OnNotImplementedCommand;
	public event Func<PrivateMessage, Task> OnPrivateMessage;
	public event Func<PrivateSystemMessage, Task> OnPrivateSystemMessage;
	public event Func<PsimData, Task> OnPsimData;
	public event Func<RawData, Task> OnRawData;
	public event Func<RoomUsers, Task> OnRoomUsers;
	public event Func<SocketConnected, Task> OnSocketConnected;
	public event Func<SocketDisconnected, Task> OnSocketDisconnected;
	public event Func<SocketError, Task> OnSocketError;

	public EventPsimClient(PsimClientOptions options) : base(options)
	{
		Subscribe(new EventPsimClientEvents(this));
	}

	private static Task Raise<TEventArgs>(Func<TEventArgs, Task> handlers, TEventArgs args)
	{
		if (handlers != null)
		{
			return Task.WhenAll(handlers.GetInvocationList()
				.OfType<Func<TEventArgs, Task>>()
				.Select(h => h(args)));
		}

		return Task.CompletedTask;
	}

	private class EventPsimClientEvents : ISubscriber<ByteBuffer>, ISubscriber<ChallengeString>,
		ISubscriber<ChatMessage>, ISubscriber<ClientJoinRoom>, ISubscriber<ClientLeaveRoom>, ISubscriber<LoginFailure>,
		ISubscriber<LoginSuccess>, ISubscriber<NotImplementedCommand>, ISubscriber<PrivateMessage>,
		ISubscriber<PrivateSystemMessage>, ISubscriber<PsimData>, ISubscriber<RawData>, ISubscriber<RoomUsers>,
		ISubscriber<SocketConnected>, ISubscriber<SocketDisconnected>, ISubscriber<SocketError>
	{
		private readonly EventPsimClient _client;

		public EventPsimClientEvents(EventPsimClient client)
		{
			_client = client;
		}

		public Task HandleEvent(ByteBuffer e) => Raise(_client.OnByteBuffer, e);
		public Task HandleEvent(ChallengeString e) => Raise(_client.OnChallengeString, e);
		public Task HandleEvent(ChatMessage e) => Raise(_client.OnChatMessage, e);
		public Task HandleEvent(ClientJoinRoom e) => Raise(_client.OnClientJoinRoom, e);
		public Task HandleEvent(ClientLeaveRoom e) => Raise(_client.OnClientLeaveRoom, e);
		public Task HandleEvent(LoginFailure e) => Raise(_client.OnLoginFailure, e);
		public Task HandleEvent(LoginSuccess e) => Raise(_client.OnLoginSuccess, e);
		public Task HandleEvent(NotImplementedCommand e) => Raise(_client.OnNotImplementedCommand, e);
		public Task HandleEvent(PrivateMessage e) => Raise(_client.OnPrivateMessage, e);
		public Task HandleEvent(PrivateSystemMessage e) => Raise(_client.OnPrivateSystemMessage, e);
		public Task HandleEvent(PsimData e) => Raise(_client.OnPsimData, e);
		public Task HandleEvent(RawData e) => Raise(_client.OnRawData, e);
		public Task HandleEvent(RoomUsers e) => Raise(_client.OnRoomUsers, e);
		public Task HandleEvent(SocketConnected e) => Raise(_client.OnSocketConnected, e);
		public Task HandleEvent(SocketDisconnected e) => Raise(_client.OnSocketDisconnected, e);
		public Task HandleEvent(SocketError e) => Raise(_client.OnSocketError, e);
	}
}