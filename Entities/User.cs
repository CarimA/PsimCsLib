using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public sealed class User
{
    private readonly PsimClient _client;
    private Dictionary<Room, Rank> _ranks;
    public IReadOnlyDictionary<Room, Rank> Rank => _ranks.AsReadOnly();
    public PsimUsername Name { get; private set; }

    internal User(PsimClient client, PsimUsername name)
    {
        _client = client;
        Name = name;
        _ranks = new Dictionary<Room, Rank>();
    }

    public async Task Send(string message)
    {
        await _client.Send($"|/w {Name.Token},{message}");
    }

    internal void Join(Room room, Rank rank)
    {
        _ranks.Add(room, rank);
    }

    internal void Leave(Room room)
    {
        _ranks.Remove(room);
    }

    internal void Rename(PsimUsername name)
    {
        Name = name;
    }
}