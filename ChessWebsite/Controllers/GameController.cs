using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ChessLibrary;
using ChessWebsite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace ChessWebsite.Controllers
{
    public class GameController : Controller
    {
        private ApplicationDbContext _context;

        public GameController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Game
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Game/Create
        [System.Web.Mvc.Authorize]
        public async Task<ActionResult> Create()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            DbSet<Game> games = _context.Games;
            var gameBoard = new GameBoard();
            string authorizedUserId = User.Identity.GetUserId();
            var createdGame = new Game
            {
                WhitePlayer = _context.Users.First(u => u.Id == authorizedUserId),
                BlackPlayer = null,
                GameBoardJson = JsonConvert.SerializeObject(gameBoard, settings)
            };
            games.Add(createdGame);
            await _context.SaveChangesAsync();
            return RedirectToAction("Id", "Game", new {id = createdGame.Id});
        }

        [System.Web.Mvc.Authorize]
        public async Task<ActionResult> Id(int id)
        {
            var games = _context.Games;
            var game = games.Include(g => g.WhitePlayer)
                            .Include(g => g.BlackPlayer)
                            .FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return HttpNotFound();
            }

            string authorizedUserId = User.Identity.GetUserId();
            
            // Opponent entered.
            if (game.BlackPlayer == null && game.WhitePlayer.Id != authorizedUserId)
            {
                game.BlackPlayer = _context.Users.First(u => u.Id == authorizedUserId);
                await _context.SaveChangesAsync();
            }

            if (game.WhitePlayer.Id == authorizedUserId || game.BlackPlayer.Id == authorizedUserId)
            {
                return View(game);
            }

            return HttpNotFound();
        }
    }
}