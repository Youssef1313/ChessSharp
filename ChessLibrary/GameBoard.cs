using System;
using System.Collections.Generic;
using System.Linq;
using ChessLibrary.Pieces;
using ChessLibrary.SquareData;

namespace ChessLibrary
{
    
    public class GameBoard
    {
        public Piece this[File file, Rank rank] => Board[(int)rank, (int)file];
        public Piece this[Square square] => this[square.File, square.Rank];

        public List<Move> Moves { get; set; }

        public GameBoard()
        {
            Moves = new List<Move>();
            var whitePawn = new Pawn(Player.White);
            var whiteRook = new Rook(Player.White);
            var whiteKnight = new Knight(Player.White);
            var whiteBishop = new Bishop(Player.White);
            var whiteQueen = new Queen(Player.White);
            var whiteKing = new King(Player.White);

            var blackPawn = new Pawn(Player.Black);
            var blackRook = new Rook(Player.Black);
            var blackKnight = new Knight(Player.Black);
            var blackBishop = new Bishop(Player.Black);
            var blackQueen = new Queen(Player.Black);
            var blackKing = new King(Player.Black);
            Board = new Piece[,]
            {
                { whiteRook, whiteKnight, whiteBishop, whiteQueen, whiteKing, whiteBishop, whiteKnight, whiteRook },
                { whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn },
                { null, null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null, null },
                { blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn},
                { blackRook, blackKnight, blackBishop, blackQueen, blackKing, blackBishop, blackKnight, blackRook}
            };
        }

        public Piece[,] Board { get; set; }

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

            return (WhoseTurn() == move.Player && piece != null && piece.Owner == move.Player && 
                    !Equals(move.Source, move.Destination) && 
                    (this[move.Destination] == null || this[move.Destination].Owner != move.Player) &&
                    !ChessUtilities.PlayerWillBeInCheck(move, Board) && piece.IsValidGameMove(move, Board));
        }
        
    }

    
}