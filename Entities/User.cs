using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public sealed class User
{
    private readonly PsimClient _client;
    private Dictionary<Room, Rank> _ranks;
    public IReadOnlyDictionary<Room, Rank> Rank => _ranks.AsReadOnly();
    public string DisplayName { get; private set; }

    internal User(PsimClient client, string displayName)
    {
        _client = client;
        DisplayName = displayName;
        _ranks = new Dictionary<Room, Rank>();
    }

    public async Task Send(string message)
    {
        await _client.Send($"|/w {DisplayName},{message}");
    }

    internal void Join(Room room, Rank rank)
    {
        _ranks.Add(room, rank);
    }

    internal void Leave(Room room)
    {
        _ranks.Remove(room);
    }

    internal void Rename(string displayName)
    {
        DisplayName = displayName;
    }
}