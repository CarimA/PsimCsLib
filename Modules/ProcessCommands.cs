using PsimCsLib.Entities;
using PsimCsLib.Models;
using PsimCsLib.PubSub;

namespace PsimCsLib.Modules;

internal class ProcessCommands : ISubscriber<PsimData>
{
	private readonly PsimClient _client;

	public ProcessCommands(PsimClient client)
	{
		_client = client;
	}

	public async Task HandleEvent(PsimData e)
	{
		var action = (Func<PsimData, Task>)(e.Command switch
		{
			"challstr" => HandleChallengeString,
			"init" => HandleClientRoomJoin,
			"deinit" => HandleClientRoomLeave,
			"raw" => HandleRaw,
			"c:" => HandleChatMessage,
			"j" => HandleJoin,
			"l" => HandleLeave,
			"n" => HandleRename,
			"pm" => HandlePrivateMessage,
			"users" => HandleUsers,
			"queryresponse" => HandleQueryResponse,
			_ => NotImplementedCommand
		});

		await action(e);
	}

	private async Task NotImplementedCommand(PsimData e)
	{
		await _client.Publish(new NotImplementedCommand(e));
	}

	private async Task HandleChallengeString(PsimData e)
	{
		await _client.Publish(new ChallengeString(e.Arguments[0], e.Arguments[1]));
	}

	private async Task HandleClientRoomJoin(PsimData e)
	{
		await _client.Publish(new ClientJoinRoom(e.Room));
	}

	private async Task HandleClientRoomLeave(PsimData e)
	{
		await _client.Publish(new ClientLeaveRoom(e.Room));
	}

	private async Task HandleRaw(PsimData e)
	{
		await _client.Publish(new RawData(e.Room, e.IsIntro, e.Arguments[0]));
	}

	private async Task HandleChatMessage(PsimData e)
	{
		var datePosted = DateTimeOffset.FromUnixTimeSeconds(long.Parse(e.Arguments[0])).LocalDateTime;
		var user = new PsimUsername(_client, e.Arguments[1]);
		await _client.Publish(new ChatMessage(e.Room, datePosted, user, e.Arguments[2], e.IsIntro));
	}

	private async Task HandleJoin(PsimData e)
	{
		var user = new PsimUsername(_client, e.Arguments[0]);
		await _client.Publish(new UserJoinRoom(e.Room, user, e.IsIntro));
	}

	private async Task HandleLeave(PsimData e)
	{
		var user = new PsimUsername(_client, e.Arguments[0]);
		await _client.Publish(new UserLeaveRoom(e.Room, user, e.IsIntro));
	}
	
	private async Task HandleRename(PsimData e)
	{
		var user = new PsimUsername(_client, e.Arguments[0]);
		await _client.Publish(new UserRename(e.Arguments[1], user, e.IsIntro));
	}

	private async Task HandlePrivateMessage(PsimData e)
	{
		if (e.Arguments[1] == "~")
		{
			await _client.Publish(new PrivateSystemMessage(e.Arguments[2]));
		}
		else
		{
			var user = new PsimUsername(_client, e.Arguments[0]);
			await _client.Publish(new PrivateMessage(user, e.Arguments[2], e.IsIntro));
		}
	}

	private async Task HandleUsers(PsimData e)
	{
		var split = e.Arguments[0].Split(',');
		var count = int.Parse(split[0]);
		var users = split.Skip(1).Select(name => new PsimUsername(_client, name)).ToList();
		await _client.Publish(new RoomUsers(e.Room, count, users));
	}

	private async Task HandleQueryResponse(PsimData e)
	{
		var action = (Func<PsimData, Task>)(e.Arguments.FirstOrDefault() switch
		{
			"userdetails" => HandleUserDetails,
		});

		await action(e).ConfigureAwait(false);
	}

	private async Task HandleUserDetails(PsimData arg)
	{
		var json = arg.Arguments.LastOrDefault();

		if (json == null)
			return;

		var details = UserDetails.FromJson(json);

		if (details == null)
			return;

		await _client.Publish(details).ConfigureAwait(false);
	}
}