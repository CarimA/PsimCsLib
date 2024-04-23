using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public class UserCollection
{
    private readonly PsimClient _client;

    private readonly Dictionary<string, RoomUser> _roomUsers;
    private readonly List<User> _users;

    public UserCollection(PsimClient client)
    {
        _client = client;
        _roomUsers = new Dictionary<string, RoomUser>();
        _users = new List<User>();
    }

    internal RoomUser FindUser(string username)
    {
        var token = Utils.ProcessName(username).TokenName;
        if (_roomUsers.TryGetValue(token, out var value))
            return value;

        var account = new User();
        var user = new RoomUser(_client, account, username);
        _roomUsers.Add(token, user);
        return user;
    }
}