﻿using System.Text.RegularExpressions;

namespace PsimCsLib.Entities;

public class PsimUsername : IEquatable<PsimUsername>
{
    public string Token { get; }
    public string DisplayName { get; }
    public bool IsIdle { get; }

    public PsimUsername(string input)
    {
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

    public override string ToString()
    {
        return DisplayName;
    }

    public static implicit operator PsimUsername(string input)
    {
        return new PsimUsername(input);
    }
}