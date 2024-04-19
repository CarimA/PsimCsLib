namespace PsimCsLib.Models;
public sealed class LoginSuccess
{
    public string ResponseString { get; }
    public string Assertion { get; }

    public LoginSuccess(string responseString, string assertion)
    {
        ResponseString = responseString;
        Assertion = assertion;
    }
}
