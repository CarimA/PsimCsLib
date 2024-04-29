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
            var token = Utils.ProcessName(key).TokenName;

            if (!_users.TryGetValue(token, out var result))
            {
                result = new User(_client, key);
                _users.TryAdd(key, result);
            }

            return result;
        }
    }
}