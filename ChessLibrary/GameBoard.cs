using System;
using System.Collections.Generic;
using System.Linq;
using ChessLibrary.Pieces;
using ChessLibrary.PositionData;

namespace ChessLibrary
{
    
    public class GameBoard
    {
        // Indexers.
        public Piece this[File file, Rank rank] => Board[(int)rank, (int)file];
        public Piece this[Position position] => this[position.File, position.Rank];

        public List<Move> Moves { get; set; }

        public GameBoard()
        {
            Moves = new List<Move>();
        }

        public Piece[,] Board { get; set; } = 
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

        public Player WhoseTurn()
        {
            return Moves.Count == 0 ? Player.White : RevertPlayer(Moves.Last().Player);
        }

        public Player RevertPlayer(Player player)
        {
            return player == Player.White ? Player.Black : Player.White;
        }

        public void MakeMove(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (Moves.Count == 0)
            {

            }
            Piece piece = this[move.Source];
            Board[(int) move.Source.Rank, (int) move.Source.File] = null;
            Board[(int) move.Destination.Rank, (int) move.Destination.File] = piece;
        }

        public bool IsValidMove(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }
            Piece piece = this[move.Source];
            return piece.IsValidGameMove(move, this);
        }
        
    }

    
}