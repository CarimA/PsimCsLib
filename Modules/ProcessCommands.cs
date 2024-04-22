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
            _ => UnhandledCommand
        });

        await action(e);
    }

    private async Task UnhandledCommand(PsimData e)
    {
        await _client.Publish(new UnhandledCommand(e));
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
}
