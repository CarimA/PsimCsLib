using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class UserUnlocked
{
    public PsimUsername User { get; }
    public bool IsIntro { get; }

    public UserUnlocked(PsimUsername user, bool isIntro)
    {
        User = user;
        IsIntro = isIntro;
    }
}