using PsimCsLib.Models;
using PsimCsLib.PubSub;

namespace PsimCsLib.Modules;
internal class UserJoinsRoom : ISubscriber<RoomUsers>
{
    private readonly PsimClient _client;

    public UserJoinsRoom(PsimClient client)
    {
        _client = client;
    }

    public async Task HandleEvent(RoomUsers e)
    {
        foreach (var (username, user) in e.Users)
        {
            user.Join(e.Room, username.Rank);
        }
    }
}
