using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public class UserLocked
{
    public PsimUsername User { get; }
    public bool IsIntro { get; private set; }

    public UserLocked(PsimUsername user, bool isIntro)
    {
        User = user;
        IsIntro = isIntro;
    }
}