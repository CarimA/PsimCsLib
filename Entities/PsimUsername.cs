using PsimCsLib.Enums;
using System.Text.RegularExpressions;

namespace PsimCsLib.Entities;

public class PsimUsername : IEquatable<PsimUsername>
{
	private readonly PsimClient _client;
	public Rank Rank { get; }
	public string Token { get; }
	public string DisplayName { get; }
	public bool IsIdle { get; }

	internal PsimUsername(PsimClient client, string input)
	{
		_client = client;
		Rank = GetRank(input);
		IsIdle = input.EndsWith("@!");
		DisplayName = input.Substring(1, input.Length - (IsIdle ? 3 : 1));
		Token = TokeniseName(input);
	}

	public static string TokeniseName(string input)
	{
		return Regex.Replace(input.ToLowerInvariant(), "[^a-z0-9]", "");
	}

	public override bool Equals(object? obj)
	{
		return obj is PsimUsername psimUsername && Equals(psimUsername);
	}

	public bool Equals(PsimUsername? other)
	{
		return Token.Equals(other?.Token, StringComparison.InvariantCultureIgnoreCase);
	}

	public override int GetHashCode()
	{
		return Token.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
	}

	public async Task Send(string message)
	{
		await _client.Send($"|/w {Token},{message}").ConfigureAwait(false);
	}

	public static Rank GetRank(string input)
	{
		return input[..1] switch
		{
			"‽" => Rank.Locked,
			"!" => Rank.Muted,
			"☆" => Rank.Battler,
			" " => Rank.Normal,
			"+" => Rank.Voice,
			"*" => Rank.Bot,
			"%" => Rank.Driver,
			"@" => Rank.Moderator,
			"&" => Rank.Administrator,
			"#" => Rank.RoomOwner,
			_ => throw new NotImplementedException($"Rank could not be found from {input}")
		};
	}

	public static string FromRank(Rank rank)
	{
		return rank switch
		{
			Rank.Locked => "‽",
			Rank.Muted => "!",
			Rank.Battler => "☆",
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