using System.Collections.Concurrent;
using PsimCsLib.Models;
using PsimCsLib.PubSub;

namespace PsimCsLib.Modules;

public class UserDetailsResolver : ISubscriber<UserDetails>
{
    private readonly ConcurrentDictionary<string, UserDetails> _userDetailsRequests;

    public UserDetailsResolver(ConcurrentDictionary<string, UserDetails> userDetailsRequests)
    {
        _userDetailsRequests = userDetailsRequests;
    }

    public Task HandleEvent(UserDetails e)
    {
	    _userDetailsRequests.TryAdd(e.UserId, e);
	    return Task.CompletedTask;
    }
}