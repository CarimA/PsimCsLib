using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed record TournamentCreated(Room Room, string Format, string Type, int PlayerCap, bool IsIntro);