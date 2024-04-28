using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using ChessSharpWeb.Data;
using ChessSharpWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChessWebsite;

public class ChessHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly string _loggedUserId;

    public ChessHub(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _loggedUserId = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }

    private Game GetGameFromId(int gameId)
    {
        return _context.Games.Include(g => g.WhitePlayer).Include(g => g.BlackPlayer).First(g => g.Id == gameId); // TODO: Can it be null ?
    }

    private static GameBoard GetGameBoardFromJson(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        return JsonConvert.DeserializeObject<GameBoard>(json, settings);
    }

    [Authorize]
    public override async Task OnConnectedAsync()
    {
        
        int gameId = Convert.ToInt32(Context.GetHttpContext()!.Request.Query["gameId"]);
        Game game = GetGameFromId(gameId);
        if (_loggedUserId == game.WhitePlayer.Id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
        else if (game.BlackPlayer != null && game.BlackPlayer.Id == _loggedUserId)
        {
            await Clients.Group(gameId.ToString()).SendAsync("BlackJoined", game.BlackPlayer.UserName);
        }
        await base.OnConnectedAsync();
    }

    public List<Move> GetValidMovesOfSquare(string square)
    {
        int gameId = Convert.ToInt32(Context.GetHttpContext()!.Request.Query["gameId"]);
        var game = GetGameFromId(gameId);
        var gameBoard = GetGameBoardFromJson(game.GameBoardJson);
        return ChessUtilities.GetValidMovesOfSourceSquare(Square.Parse(square), gameBoard);
    }

    public async Task MakeMove(int gameId, string source, string destination, int? promoteTo = null)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        var game = _context.Games.Include(m => m.WhitePlayer)
                                 .Include(m => m.WhitePlayer)
                                 .FirstOrDefault(g => g.Id == gameId);
        if (game == null)
        {
            // No game exists.
            // Not sure what to do here, temporarily, I'll return.
            return;
        }



        if (!Regex.IsMatch(source, "[A-H][1-8]", RegexOptions.IgnoreCase) ||
            !Regex.IsMatch(destination, "[A-H][1-8]", RegexOptions.IgnoreCase))
        {
            // Invalid squares.
            return;
        }


        /*
         * promoteTo: 0 ==> Knight,
         * promoteTo: 1 ==> Bishop,
         * promoteTo: 2 ==> Rook,
         * promoteTo: 3 ==> Queen
         */
        if (promoteTo != null && (promoteTo < 0 || promoteTo > 3))
        {
            // Invalid promoteTo value
            return;
        }


        var gameBoard = JsonConvert.DeserializeObject<GameBoard>(game.GameBoardJson, settings);
        var move = new Move(Square.Parse(source), Square.Parse(destination), gameBoard.WhoseTurn(),
            (PawnPromotion?)promoteTo);

        if (gameBoard.IsValidMove(move))
        {
            gameBoard.MakeMove(move, true);
            game.GameBoardJson = JsonConvert.SerializeObject(gameBoard, settings);
            await _context.SaveChangesAsync();
            await Clients.Users(new List<string>() { game.WhitePlayer.Id, game.BlackPlayer!.Id }).SendAsync("ShowGame", gameBoard.Board);
        }
    }

}
