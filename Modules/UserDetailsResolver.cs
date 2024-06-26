using PsimCsLib.Models;
using PsimCsLib.PubSub;

namespace PsimCsLib.Modules;

public class UserDetailsResolver : ISubscriber<UserDetails>
{
	private readonly Dictionary<string, TaskCompletionSource<UserDetails>> _userDetailsRequests;

	public UserDetailsResolver(Dictionary<string, TaskCompletionSource<UserDetails>> userDetailsRequests)
	{
		_userDetailsRequests = userDetailsRequests;
	}

	public Task HandleEvent(UserDetails e)
	{
		if (_userDetailsRequests.TryGetValue(e.UserId, out var tcs))
		{
			tcs.SetResult(e);
			_userDetailsRequests.Remove(e.UserId);
		}

		return Task.CompletedTask;
	}
}