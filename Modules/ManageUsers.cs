using PsimCsLib.Entities;
using PsimCsLib.Models;
using PsimCsLib.PubSub;

namespace PsimCsLib.Modules;
internal class ManageUsers : ISubscriber<UserJoinRoom>, ISubscriber<UserLeaveRoom>, ISubscriber<RoomUsers>
{
	public Task HandleEvent(UserJoinRoom e)
	{
		AddUser(e.Room, e.User);
		return Task.CompletedTask;
	}

	public Task HandleEvent(UserLeaveRoom e)
	{
		e.Room.Users.RemoveAll(user => user.Token == e.User.Token);
		return Task.CompletedTask;
	}

	public Task HandleEvent(RoomUsers e)
	{
		foreach (var user in e.Users)
			AddUser(e.Room, user);

		return Task.CompletedTask;
	}

	private void AddUser(Room room, PsimUsername psimUser)
	{
		if (room.Users.All(user => user.Token != psimUser.Token))
			room.Users.Add(psimUser);
	}
}
