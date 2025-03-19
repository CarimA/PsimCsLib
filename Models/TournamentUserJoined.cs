using PsimCsLib.Entities;

namespace PsimCsLib.Models;

public sealed record TournamentUserJoined(Room Room, PsimUsername User, bool IsIntro);