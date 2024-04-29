using System.Collections.Concurrent;

namespace PsimCsLib.Entities;

public sealed class UserCollection
{
    private readonly PsimClient _client;
    private readonly ConcurrentDictionary<PsimUsername, User> _users;

    internal UserCollection(PsimClient client)
    {
        _client = client;
        _users = new ConcurrentDictionary<PsimUsername, User>();
    }

    internal User this[PsimUsername key]
    {
        get
        {
            if (!_users.TryGetValue(key, out var result))
            {
                result = new User(_client, key);
                _users.TryAdd(key, result);
            }

            return result;
        }
    }

    internal User this[RankPsimUsername key]
    {
        get
        {
            if (!_users.TryGetValue(key, out var result))
            {
                result = new User(_client, key);
                _users.TryAdd(key, result);
            }

            return result;
        }
    }

    internal void RenameUser(PsimUsername oldName, PsimUsername newName)
    {
        if (_users.TryRemove(oldName, out var result))
        {
            result.Rename(newName);
            _users.TryAdd(newName, result);
        }
    }
}