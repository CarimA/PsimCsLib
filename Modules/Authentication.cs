using Newtonsoft.Json.Linq;
using PsimCsLib.Entities;
using PsimCsLib.Models;
using PsimCsLib.PubSub;

namespace PsimCsLib.Modules;
internal class Authentication : ISubscriber<ChallengeString>, ISubscriber<LoginSuccess>, ISubscriber<LoginFailure>
{
	private readonly PsimClient _client;

	public Authentication(PsimClient client)
	{
		_client = client;
	}

	public async Task HandleEvent(ChallengeString e)
	{
		using var httpClient = new HttpClient();
		var content = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			{ "act", "login" },
			{ "name", PsimUsername.TokeniseName(_client.Options.Username) },
			{ "pass", _client.Options.Password },
			{ "challengekeyid", e.Id },
			{ "challenge", e.Challenge }
		});

		var response = await httpClient.PostAsync(_client.Options.LoginServer, content);
		var responseString = (await response.Content.ReadAsStringAsync())[1..];
		var obj = JObject.Parse(responseString);
		var assertion = (obj.GetValue("assertion") ?? string.Empty).ToString();
		var success = (obj.GetValue("actionsuccess")?.Value<bool>() ?? false)
			&& !string.IsNullOrEmpty(assertion);

		if (!success)
		{
			var error = (obj.GetValue("actionerror") ?? string.Empty).ToString();
			await _client.Publish(new LoginFailure(responseString, error));
			return;
		}

		await _client.Publish(new LoginSuccess(responseString, assertion));
	}

	public async Task HandleEvent(LoginSuccess e)
	{
		await _client.ForceSend($"|/trn {_client.Options.Username},0,{e.Assertion}");
		_client.LoggedIn = true;
	}

	public async Task HandleEvent(LoginFailure e)
	{
		_client.Disconnect($"{e.Reason} ({e.ResponseString})");
	}
}
