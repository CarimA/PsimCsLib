namespace PsimCsLib.Models;
public sealed class LoginSuccess
{
    public string ResponseString { get; }
    public string Assertion { get; }

    internal LoginSuccess(string responseString, string assertion)
    {
        ResponseString = responseString;
        Assertion = assertion;
    }
}
