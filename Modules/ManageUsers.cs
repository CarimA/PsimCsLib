using PsimCsLib.Entities;
using PsimCsLib.Enums;
using PsimCsLib.Models;
using PsimCsLib.PubSub;
using System.Text.RegularExpressions;

namespace PsimCsLib.Modules;
internal class ManageUsers : ISubscriber<UserJoinRoom>, ISubscriber<UserLeaveRoom>, ISubscriber<RoomUsers>, ISubscriber<UserRename>, ISubscriber<ChatMessage>
{
	private readonly PsimClient _client;

	public ManageUsers(PsimClient client)
	{
		_client = client;
	}

	public Task HandleEvent(UserJoinRoom e)
	{
		AddUser(e.Room, e.User);
		return Task.CompletedTask;
	}

	public Task HandleEvent(UserLeaveRoom e)
	{
		RemoveUser(e.Room, e.User.Token);
		return Task.CompletedTask;
	}

	public Task HandleEvent(RoomUsers e)
	{
		foreach (var user in e.Users)
			AddUser(e.Room, user);

		return Task.CompletedTask;
	}

	public async Task HandleEvent(UserRename e)
	{
		var user = GetUser(e.Room, e.OldId);

		if (user == null)
			return;

		// user is muted
		if (user.Rank != Rank.Muted && e.User.Rank == Rank.Muted)
			await _client.Publish(new UserMuted(e.Room, user, e.IsIntro));

		// user is unmuted
		if (user.Rank == Rank.Muted && e.User.Rank != Rank.Muted)
			await _client.Publish(new UserUnmuted(e.Room, user, e.IsIntro));

		// user is locked
		if (user.Rank != Rank.Locked && e.User.Rank == Rank.Locked)
			await _client.Publish(new UserLocked(user, e.IsIntro));

		// user is unlocked
		if (user.Rank == Rank.Muted && e.User.Rank != Rank.Muted)
			await _client.Publish(new UserUnlocked(user, e.IsIntro));

		user.DisplayName = e.User.DisplayName;
		user.Token = e.User.Token;
		user.IsIdle = e.User.IsIdle;
		user.Rank = e.User.Rank;
	}

	private PsimUsername? GetUser(Room room, string token)
	{
		return room.Users.FirstOrDefault(user => user.Token == token);
	}

	private void AddUser(Room room, PsimUsername psimUser)
	{
		if (room.Users.All(user => user.Token != psimUser.Token))
			room.Users.Add(psimUser);
	}

	private void RemoveUser(Room room, string token)
	{
		room.Users.RemoveAll(user => user.Token == token);
	}

	public async Task HandleEvent(ChatMessage e)
	{
		if (!e.Message.StartsWith("/log"))
			return;

		var action = Regex.Match(e.Message, "/log (.+) was (.+) from (.+) by (\\w+)\\.");

		if (!action.Success)
			return;

		var room = e.Room;
		var user = action.Captures[0].Value;
		var result = action.Captures[1].Value;
		var staff = action.Captures[3].Value;

		var userToken = PsimUsername.TokeniseName(user);
		var staffToken = PsimUsername.TokeniseName(staff);

		var userObj = e.Room.Users.FirstOrDefault(u => u.Token == userToken);
		var staffObj = e.Room.Users.FirstOrDefault(u => u.Token == staffToken);

		if (userObj == null || staffObj == null)
			return;

		if (result == "banned")
			await _client.Publish(new UserBanned(userObj, room, staffObj, e.DatePosted, e.IsIntro));
		else if (result == "unbanned")
			await _client.Publish(new UserUnbanned(userObj, room, staffObj, e.DatePosted, e.IsIntro));
	}
}