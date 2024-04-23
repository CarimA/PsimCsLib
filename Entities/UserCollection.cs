using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public class UserCollection
{
    private readonly PsimClient _client;
    private readonly Dictionary<string, User> _users;

    public UserCollection(PsimClient client)
    {
        _client = client;
        _users = new Dictionary<string, User>();
    }

    public User this[string key]
    {
        get
        {
            var token = Utils.ProcessName(key).TokenName;

            if (!_users.TryGetValue(token, out var result))
            {
                result = new User(key);
                _users.Add(token, result);
            }

            return result;
        }
    }

    internal void Rename(string oldName, string newName)
    {
        var oldToken = Utils.ProcessName(oldName).TokenName;
        var newToken = Utils.ProcessName(newName).TokenName;

        if (!_users.TryGetValue(oldToken, out var result)) 
            return;

        _users.Remove(oldToken);
        _users.Add(newToken, result);
        result.Rename(newName);
    }
}