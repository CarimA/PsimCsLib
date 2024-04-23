using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public class RoomUser
{
    private readonly PsimClient _client;
    public string DisplayName { get; }
    public string TokenName { get;  }
    public Rank Rank { get; }
    public User Account { get; }

    internal RoomUser(PsimClient client, User user, string name)
    {
        _client = client;
        Account = user;
        (Rank, TokenName, DisplayName) = Utils.ProcessName(name);
    }

    public async Task Send(string message)
    {
        await _client.Send($"|/w {TokenName},{message}");
    }
}