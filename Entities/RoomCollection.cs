namespace PsimCsLib.Entities;

public class RoomCollection
{
	private readonly PsimClient _client;
	private readonly Dictionary<string, Room> _rooms;

	internal RoomCollection(PsimClient client)
	{
		_client = client;
		_rooms = new Dictionary<string, Room>();
	}

	public Room this[string key]
	{
		get
		{
			if (!_rooms.TryGetValue(key, out var result))
			{
				result = new Room(_client, key);
				_rooms.Add(key, result);
			}

			return result;
		}
	}

	public async Task<Room> Join(string room)
	{
		await _client.Send($"|/join {room}");
		return this[room];
	}

	public async Task Leave(string room)
	{
		await _client.Send($"|/leave {room}");
	}
}