using Microsoft.AspNetCore.Identity;

namespace ChessSharpWeb.Models;

public class Game
{
    public int Id { get; set; }
    public IdentityUser WhitePlayer { get; set; } = null!;
    public IdentityUser? BlackPlayer { get; set; }
    public string GameBoardJson { get; set; } = null!;

}
