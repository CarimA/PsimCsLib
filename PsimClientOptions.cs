namespace PsimCsLib;

public sealed class PsimClientOptions
{
	public string Username { get; set; }
	public string Password { get; set; }
	public string ServerAddress { get; set; } = "sim3.psim.us";
	public string LoginServer { get; set; } = "https://play.pokemonshowdown.com/~~showdown/action.php";
	public bool SecureWebsocketConnection { get; set; } = true;
	public int Port { get; set; } = 443;

	internal string ToServerUri()
	{
		var socket = SecureWebsocketConnection ? "wss" : "ws";
		return $"{socket}://{ServerAddress}:{Port}/showdown/websocket";
	}
}