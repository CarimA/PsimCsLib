using PsimCsLib.Enums;

namespace PsimCsLib.Entities;

public class RankPsimUsername : PsimUsername
{
    public Rank Rank { get; }

    public RankPsimUsername(string input) : base(input)
    {
        Rank = GetRank(input);
    }

    public static Rank GetRank(string input)
    {
        return input[..1] switch
        {
            "‽" => Enums.Rank.Locked,
            "!" => Enums.Rank.Muted,
            " " => Enums.Rank.Normal,
            "+" => Enums.Rank.Voice,
            "*" => Enums.Rank.Bot,
            "%" => Enums.Rank.Driver,
            "@" => Enums.Rank.Moderator,
            "&" => Enums.Rank.Administrator,
            "#" => Enums.Rank.RoomOwner,
            _ => throw new NotImplementedException($"Rank could not be found from {input}")
        };
    }

    public static string FromRank(Rank rank)
    {
        return rank switch
        {
            Rank.Locked => "‽",
            Rank.Muted => "!",
            Rank.Normal => string.Empty,
            Rank.Voice => "+",
            Rank.Bot => "*",
            Rank.Driver => "%",
            Rank.Moderator => "@",
            Rank.Administrator => "&",
            Rank.RoomOwner => "#",
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
        };
    }

    public override string ToString()
    {
        return $"{FromRank(Rank)}{DisplayName}";
    }
}