using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using ChessSharpWeb.Data;
using ChessSharpWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChessWebsite
{
    public class ChessHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChessHub(ApplicationDbContext context)
        {
            _context = context;
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

        /*

        [Authorize]
        public override Task OnConnected()
        {
            var gameId = int.Parse(Context.QueryString["gameId"]);
            var userId = Context.User.Identity.GetUserId();
            var game = GetGameFromId(gameId);
            if (userId == game.WhitePlayer.Id)
            {
                Groups.Add(Context.ConnectionId, Context.QueryString["gameId"]);
            }
            else if (game.BlackPlayer != null && game.BlackPlayer.Id == userId)
            {
                Groups.Add(Context.ConnectionId, Context.QueryString["gameId"]);
                Clients.Group(gameId.ToString()).blackJoined(game.BlackPlayer.UserName);
            }
            return base.OnConnected();
        }

        public List<Move> GetValidMovesOfSquare(string square)
        {
            var gameId = int.Parse(Context.QueryString["gameId"]);
            var game = GetGameFromId(gameId);
            var gameBoard = GetGameBoardFromJson(game.GameBoardJson);
            return ChessUtilities.GetValidMovesOfSourceSquare(Square.Parse(square), gameBoard);
            //return square;
        }
*/
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
                //Clients.Users(new List<string>() { game.WhitePlayer.Id, game.BlackPlayer.Id }).showGame(gameBoard.Board);
            }
        }

    }
}