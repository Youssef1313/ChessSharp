using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChessWebsite.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
    }
}