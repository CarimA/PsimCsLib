using System.Xml.Linq;

namespace PsimCsLib.Entities;

public class User
{
    private readonly List<RoomUser> _aliases;
    public IReadOnlyList<RoomUser> Aliases => _aliases.AsReadOnly();

    internal User()
    {
        _aliases = new List<RoomUser>();
    }

    internal void AddAlias(RoomUser user)
    {
        _aliases.Add(user);
    }
}