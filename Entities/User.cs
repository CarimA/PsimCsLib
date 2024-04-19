using System.Collections.ObjectModel;

namespace PsimCsLib.Entities;

public class User
{
    public string Name { get; }

    private readonly List<Room> _rooms;
    public ReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();
}