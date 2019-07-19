using System;
using ChessLibrary.PositionData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessLibrary.Pieces;
namespace ChessLibrary.UnitTests
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void IndexerTest()
        {
            var x = new GameBoard();

            Assert.AreEqual(x[File.A, Rank.First], new Rook { Owner = Player.White });
            Assert.AreEqual(x[File.B, Rank.First], new Knight { Owner = Player.White });
            Assert.AreEqual(x[File.C, Rank.First], new Bishop { Owner = Player.White });
            Assert.AreEqual(x[File.D, Rank.First], new Queen { Owner = Player.White });
            Assert.AreEqual(x[File.E, Rank.First], new King { Owner = Player.White });
            Assert.AreEqual(x[File.F, Rank.First], new Bishop { Owner = Player.White });
            Assert.AreEqual(x[File.G, Rank.First], new Knight { Owner = Player.White });
            Assert.AreEqual(x[File.H, Rank.First], new Rook { Owner = Player.White });

            Assert.AreEqual(x[File.A, Rank.Second], new Pawn { Owner = Player.White });
            Assert.AreEqual(x[File.B, Rank.Second], new Pawn { Owner = Player.White });
            Assert.AreEqual(x[File.C, Rank.Second], new Pawn { Owner = Player.White });
            Assert.AreEqual(x[File.D, Rank.Second], new Pawn { Owner = Player.White });
            Assert.AreEqual(x[File.E, Rank.Second], new Pawn { Owner = Player.White });
            Assert.AreEqual(x[File.F, Rank.Second], new Pawn { Owner = Player.White });
            Assert.AreEqual(x[File.G, Rank.Second], new Pawn { Owner = Player.White });
            Assert.AreEqual(x[File.H, Rank.Second], new Pawn { Owner = Player.White });

            Assert.AreEqual(x[File.A, Rank.Third], null);
            Assert.AreEqual(x[File.B, Rank.Third], null);
            Assert.AreEqual(x[File.C, Rank.Third], null);
            Assert.AreEqual(x[File.D, Rank.Third], null);
            Assert.AreEqual(x[File.E, Rank.Third], null);
            Assert.AreEqual(x[File.F, Rank.Third], null);
            Assert.AreEqual(x[File.G, Rank.Third], null);
            Assert.AreEqual(x[File.H, Rank.Third], null);

            Assert.AreEqual(x[File.A, Rank.Forth], null);
            Assert.AreEqual(x[File.B, Rank.Forth], null);
            Assert.AreEqual(x[File.C, Rank.Forth], null);
            Assert.AreEqual(x[File.D, Rank.Forth], null);
            Assert.AreEqual(x[File.E, Rank.Forth], null);
            Assert.AreEqual(x[File.F, Rank.Forth], null);
            Assert.AreEqual(x[File.G, Rank.Forth], null);
            Assert.AreEqual(x[File.H, Rank.Forth], null);

            Assert.AreEqual(x[File.A, Rank.Fifth], null);
            Assert.AreEqual(x[File.B, Rank.Fifth], null);
            Assert.AreEqual(x[File.C, Rank.Fifth], null);
            Assert.AreEqual(x[File.D, Rank.Fifth], null);
            Assert.AreEqual(x[File.E, Rank.Fifth], null);
            Assert.AreEqual(x[File.F, Rank.Fifth], null);
            Assert.AreEqual(x[File.G, Rank.Fifth], null);
            Assert.AreEqual(x[File.H, Rank.Fifth], null);

            Assert.AreEqual(x[File.A, Rank.Sixth], null);
            Assert.AreEqual(x[File.B, Rank.Sixth], null);
            Assert.AreEqual(x[File.C, Rank.Sixth], null);
            Assert.AreEqual(x[File.D, Rank.Sixth], null);
            Assert.AreEqual(x[File.E, Rank.Sixth], null);
            Assert.AreEqual(x[File.F, Rank.Sixth], null);
            Assert.AreEqual(x[File.G, Rank.Sixth], null);
            Assert.AreEqual(x[File.H, Rank.Sixth], null);

            Assert.AreEqual(x[File.A, Rank.Seventh], new Pawn { Owner = Player.Black });
            Assert.AreEqual(x[File.B, Rank.Seventh], new Pawn { Owner = Player.Black });
            Assert.AreEqual(x[File.C, Rank.Seventh], new Pawn { Owner = Player.Black });
            Assert.AreEqual(x[File.D, Rank.Seventh], new Pawn { Owner = Player.Black });
            Assert.AreEqual(x[File.E, Rank.Seventh], new Pawn { Owner = Player.Black });
            Assert.AreEqual(x[File.F, Rank.Seventh], new Pawn { Owner = Player.Black });
            Assert.AreEqual(x[File.G, Rank.Seventh], new Pawn { Owner = Player.Black });
            Assert.AreEqual(x[File.H, Rank.Seventh], new Pawn { Owner = Player.Black });

            Assert.AreEqual(x[File.A, Rank.Eighth], new Rook { Owner = Player.Black });
            Assert.AreEqual(x[File.B, Rank.Eighth], new Knight { Owner = Player.Black });
            Assert.AreEqual(x[File.C, Rank.Eighth], new Bishop { Owner = Player.Black });
            Assert.AreEqual(x[File.D, Rank.Eighth], new Queen { Owner = Player.Black });
            Assert.AreEqual(x[File.E, Rank.Eighth], new King { Owner = Player.Black });
            Assert.AreEqual(x[File.F, Rank.Eighth], new Bishop { Owner = Player.Black });
            Assert.AreEqual(x[File.G, Rank.Eighth], new Knight { Owner = Player.Black });
            Assert.AreEqual(x[File.H, Rank.Eighth], new Rook { Owner = Player.Black });


        }
    }
}
