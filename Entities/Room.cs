namespace PsimCsLib.Entities;

public class Room
{
	private readonly PsimClient _client;
	public string Name { get; }
	public List<PsimUsername> Users { get; internal set; }

	internal Room(PsimClient client, string name)
	{
		_client = client;
		Name = name;
		Users = new List<PsimUsername>();
	}

	public async Task Send(string message)
	{
		await _client.Send($"{Name}|{message}");
	}

	public async Task SendHtml(string id, string text)
	{
		await Send($"/adduhtml {id},{text}");
	}
}