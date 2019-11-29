using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessSharp;

namespace ChessWebsite.Models
{
    public class Game
    {
        public int Id { get; set; }
        public ApplicationUser WhitePlayer { get; set; }
        public ApplicationUser BlackPlayer { get; set; }
        public string GameBoardJson { get; set; }

    }
}