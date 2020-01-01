using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessSharp;
using Microsoft.AspNetCore.Identity;

namespace ChessSharpWeb.Models
{
    public class Game
    {
        public int Id { get; set; }
        public IdentityUser WhitePlayer { get; set; }
        public IdentityUser BlackPlayer { get; set; }
        public string GameBoardJson { get; set; }

    }
}