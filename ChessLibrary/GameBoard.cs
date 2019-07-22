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
            { new Rook(Player.White), new Knight(Player.White), new Bishop(Player.White), new Queen(Player.White), new King(Player.White), new Bishop(Player.White), new Knight(Player.White), new Rook(Player.White) },
            { new Pawn(Player.White), new Pawn(Player.White), new Pawn(Player.White), new Pawn(Player.White), new Pawn(Player.White), new Pawn(Player.White), new Pawn(Player.White), new Pawn(Player.White) },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { new Pawn(Player.Black), new Pawn(Player.Black), new Pawn(Player.Black), new Pawn(Player.Black), new Pawn(Player.Black), new Pawn(Player.Black), new Pawn(Player.Black), new Pawn(Player.Black)},
            { new Rook(Player.Black), new Knight(Player.Black), new Bishop(Player.Black), new Queen(Player.Black), new King(Player.Black), new Bishop(Player.Black), new Knight(Player.Black), new Rook(Player.Black)}
        };

        public Player WhoseTurn()
        {
            return Moves.Count == 0 ? Player.White : ChessUtilities.RevertPlayer(Moves.Last().Player);
        }


        public void MakeMove(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (WhoseTurn() != move.Player)
            {
                throw new InvalidOperationException("Invalid player turn.");
            }
            Piece piece = this[move.Source];
            Board[(int) move.Source.Rank, (int) move.Source.File] = null;
            Board[(int) move.Destination.Rank, (int) move.Destination.File] = piece;
            Moves.Add(move);
        }

        public bool IsValidMove(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }
            Piece piece = this[move.Source];

            return (piece != null && piece.Owner == move.Player && !Equals(move.Source, move.Destination) &&
                    (this[move.Destination] == null || this[move.Destination].Owner != move.Player) &&
                    !ChessUtilities.PlayerWillBeInCheck(move, Board) && piece.IsValidGameMove(move, this));
        }
        
    }

    
}