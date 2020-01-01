using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using ChessSharp;
using ChessSharpWeb.Data;
using ChessSharpWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChessSharpWeb.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _loggedUserId;

        public GameController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _loggedUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        // GET: Game
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Game/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            DbSet<Game> games = _context.Games;
            var gameBoard = new GameBoard();
            var createdGame = new Game
            {
                WhitePlayer = _context.Users.First(u => u.Id == _loggedUserId),
                BlackPlayer = null,
                GameBoardJson = JsonConvert.SerializeObject(gameBoard, settings)
            };
            games.Add(createdGame);
            await _context.SaveChangesAsync();
            return RedirectToAction("Id", "Game", new { id = createdGame.Id });
        }

        [Authorize]
        public async Task<ActionResult> Id(int id)
        {
            var games = _context.Games;
            var game = games.Include(g => g.WhitePlayer)
                            .Include(g => g.BlackPlayer)
                            .FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            // Opponent entered.
            if (game.BlackPlayer == null && game.WhitePlayer.Id != _loggedUserId)
            {
                game.BlackPlayer = _context.Users.First(u => u.Id == _loggedUserId);
                await _context.SaveChangesAsync();
            }
            if (game.WhitePlayer.Id == _loggedUserId || game.BlackPlayer.Id == _loggedUserId)
            {
                ViewBag.UserId = _loggedUserId;
                return View(game);
            }

            return NotFound();
        }
    }
}