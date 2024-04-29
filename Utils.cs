using PsimCsLib.Enums;
using System.Text.RegularExpressions;

namespace PsimCsLib;

public static class Utils
{
    public static (Rank Rank, string TokenName, string DisplayName) ProcessName(string input)
    {
        var rank = ToRank(input);
        var displayName = input[1..];
        var username = SanitiseName(displayName);

        return (rank, username, displayName);
    }

    public static string SanitiseName(string input)
    {
        return Regex.Replace(input.ToLowerInvariant(), "[^a-z0-9]", "");
    }

    public static Rank ToRank(string rank)
    {
        return rank[..1] switch
        {
            "‽" => Rank.Locked,
            "!" => Rank.Muted,
            " " => Rank.Normal,
            "+" => Rank.Voice,
            "*" => Rank.Bot,
            "%" => Rank.Driver,
            "@" => Rank.Moderator,
            "&" => Rank.Administrator,
            "#" => Rank.RoomOwner,
            _ => throw new NotImplementedException($"Rank {rank} is not known")
        };
    }
}