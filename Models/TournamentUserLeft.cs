using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed record TournamentUserLeft(Room Room, PsimUsername User, bool IsIntro);