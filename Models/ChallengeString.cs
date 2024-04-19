namespace PsimCsLib.Models;
public sealed class ChallengeString
{
    public string Id { get; }
    public string Challenge { get; }

    public ChallengeString(string id, string challenge)
    {
        Id = id;
        Challenge = challenge;
    }
}
