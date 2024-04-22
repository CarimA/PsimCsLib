namespace PsimCsLib.Entities;

public class UserCollection
{
    private readonly PsimClient _client;
    private readonly List<User> _users;

    public UserCollection(PsimClient client)
    {
        _client = client;
        _users = new List<User>();
    }
}