using System.Collections.Concurrent;

namespace PsimCsLib.Entities;

public sealed class UserCollection
{
    private readonly PsimClient _client;
    private readonly ConcurrentDictionary<string, User> _users;

    internal UserCollection(PsimClient client)
    {
        _client = client;
        _users = new ConcurrentDictionary<string, User>();
    }

    internal User this[string key]
    {
        get
        {
            var (_, TokenName, DisplayName) = Utils.ProcessName(key);

            if (!_users.TryGetValue(TokenName, out var result))
            {
                result = new User(_client, DisplayName);
                _users.TryAdd(key, result);
            }

            return result;
        }
    }

    internal void RenameUser(string oldName, string newName)
    {
        var token = Utils.SanitiseName(oldName);
        if (_users.TryRemove(token, out var result))
        {
            result.Rename(newName);
            _users.TryAdd(Utils.SanitiseName(newName), result);
        }
    }
}