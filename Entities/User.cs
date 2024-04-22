using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public class User
{
    public string DisplayName { get; }
    public string TokenName { get; }

    private readonly Dictionary<Room, Rank> _rank;
    public IReadOnlyDictionary<Room, Rank> Rank => _rank.AsReadOnly();

    public User()
    {
        _rank = new Dictionary<Room, Rank>();
    }
}