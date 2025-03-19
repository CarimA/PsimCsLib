using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed record TournamentAutoStartEnabled(Room Room, TimeSpan TimeRemaining, bool IsIntro);