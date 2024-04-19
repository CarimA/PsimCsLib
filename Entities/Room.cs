namespace PsimCsLib.Entities;

public class Room
{
    private readonly PsimClient _client;
    public string Name { get; }

    public Room(PsimClient client, string name)
    {
        _client = client;
        Name = name;
    }

    public async Task Send(string message)
    {
        await _client.Send($"{Name}|{message}");
    }
}