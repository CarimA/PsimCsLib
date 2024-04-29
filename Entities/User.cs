using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public sealed class User
{
    private readonly PsimClient _client;
    private Dictionary<Room, Rank> _ranks;
    public IReadOnlyDictionary<Room, Rank> Rank => _ranks.AsReadOnly();
    public string DisplayName { get; }
    public string TokenName { get; }

    internal User(PsimClient client, string name)
    {
        _client = client;
        (_, TokenName, DisplayName) = Utils.ProcessName(name);
    }

    public async Task Send(string message)
    {
        await _client.Send($"|/w {TokenName},{message}");
    }

    internal void Join(Room room, Rank rank)
    {
        _ranks.Add(room, rank);
    }

    internal void Leave(Room room)
    {
        _ranks.Remove(room);
    }
}