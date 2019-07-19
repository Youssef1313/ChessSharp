using ChessLibrary.Pieces;
using ChessLibrary.PositionData;

namespace ChessLibrary
{
    
    public class GameBoard
    {
        // Indexer.
        public Piece this[File file, Rank rank] => Board[(int)rank, (int)file];

        private const int NumberOfFiles = 8;
        private const int NumberOfRanks = 8;
        public Piece[,] Board { get; set; } = new Piece[NumberOfRanks, NumberOfFiles]
        {
            { new Rook {Owner = Player.White}, new Knight {Owner = Player.White}, new Bishop {Owner = Player.White}, new Queen {Owner = Player.White}, new King {Owner = Player.White}, new Bishop {Owner = Player.White}, new Knight {Owner = Player.White}, new Rook {Owner = Player.White} },
            { new Pawn {Owner = Player.White}, new Pawn {Owner = Player.White}, new Pawn {Owner = Player.White}, new Pawn {Owner = Player.White}, new Pawn {Owner = Player.White}, new Pawn {Owner = Player.White}, new Pawn {Owner = Player.White}, new Pawn {Owner = Player.White} },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { new Pawn {Owner = Player.Black}, new Pawn {Owner = Player.Black}, new Pawn {Owner = Player.Black}, new Pawn {Owner = Player.Black}, new Pawn {Owner = Player.Black}, new Pawn {Owner = Player.Black}, new Pawn {Owner = Player.Black}, new Pawn {Owner = Player.Black} },
            { new Rook {Owner = Player.Black}, new Knight {Owner = Player.Black}, new Bishop {Owner = Player.Black}, new Queen {Owner = Player.Black}, new King {Owner = Player.Black}, new Bishop {Owner = Player.Black}, new Knight {Owner = Player.Black}, new Rook {Owner = Player.Black} }
        };

        
    }

    
}