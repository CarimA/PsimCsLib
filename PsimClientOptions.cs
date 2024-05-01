namespace PsimCsLib;

public sealed class PsimClientOptions
{
	public string Username { get; set; }
	public string Password { get; set; }
	public string ServerAddress { get; set; } = "sim.smogon.com";
	public string LoginServer { get; set; } = "https://play.pokemonshowdown.com/~~showdown/action.php";
	public bool SecureWebsocketConnection { get; set; } = false;
	public int Port { get; set; } = 8000;

	internal string ToServerUri()
	{
		var socket = SecureWebsocketConnection ? "wss" : "ws";
		return $"{socket}://{ServerAddress}:{Port}/showdown/websocket";
	}
}