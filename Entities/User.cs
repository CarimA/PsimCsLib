using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public class User
{
    public string DisplayName { get; private set; }
    public string TokenName { get; private set; }

    private readonly Dictionary<Room, Rank> _rank;
    public IReadOnlyDictionary<Room, Rank> Rank => _rank.AsReadOnly();

    public User(string name)
    {
        var user = Utils.ProcessName(name);
        DisplayName = user.DisplayName;
        TokenName = user.TokenName;
        _rank = new Dictionary<Room, Rank>();
    }

    internal void Join(Room room, Rank rank)
    {
        _rank.Add(room, rank);
    }

    internal void Leave(Room room)
    {
        _rank.Remove(room);
    }

    internal void Rename(string name)
    {
        var user = Utils.ProcessName(name);
        DisplayName = user.DisplayName;
        TokenName = user.TokenName;
    }
}