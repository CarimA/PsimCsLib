namespace PsimCsLib.Models;
public sealed class LoginFailure
{
    public string ResponseString { get; }
    public string Reason { get; }

    public LoginFailure(string responseString, string reason)
    {
        ResponseString = responseString;
        Reason = reason;
    }
}
