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
            "pm" => HandlePrivateMessage,
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
        var user = _client.Users.FindUser(e.Arguments[1]);
        await _client.Publish(new ChatMessage(e.Room, datePosted, user, e.Arguments[2], e.IsIntro));
    }

    private async Task HandlePrivateMessage(PsimData e)
    {
        if (e.Arguments[1] == "~")
        {
            await _client.Publish(new PrivateSystemMessage(e.Arguments[2]));
        }
        else
        {
            var user = _client.Users.FindUser(e.Arguments[1]);
            await _client.Publish(new PrivateMessage(user, e.Arguments[2], e.IsIntro));
        }
    }
}
