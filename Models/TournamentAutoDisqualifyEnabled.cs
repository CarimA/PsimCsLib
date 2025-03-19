using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed record TournamentAutoDisqualifyEnabled(Room Room, TimeSpan TimeRemaining, bool IsIntro);