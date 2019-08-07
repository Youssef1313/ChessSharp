using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ChessLibrary.Pieces;
using ChessLibrary.SquareData;

namespace ChessLibrary
{
    
    public class GameBoard
    {
        public Piece this[File file, Rank rank] => Board[(int)rank, (int)file];
        public Piece this[Square square] => this[square.File, square.Rank];

        public List<Move> Moves { get; set; }
        public Piece[,] Board { get; set; }

        public bool IsWhiteQueenSideRookMoved { get; private set; }
        public bool IsWhiteKingSideRookMoved { get; private set; }
        public bool IsWhiteKingMoved { get; private set; }

        public bool IsBlackQueenSideRookMoved { get; private set; }
        public bool IsBlackKingSideRookMoved { get; private set; }
        public bool IsBlackKingMoved { get; private set; }

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

            if (piece == null)
            {
                throw new InvalidOperationException("Source square has no piece.");
            }

            if (piece.Owner == Player.White && piece.GetType().Name == typeof(King).Name)
            {
                IsWhiteKingMoved = true;
            }

            if (piece.Owner == Player.White && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.A && move.Source.Rank == Rank.First)
            {
                IsWhiteQueenSideRookMoved = true;
            }

            if (piece.Owner == Player.White && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.H && move.Source.Rank == Rank.First)
            {
                IsWhiteKingSideRookMoved = true;
            }

            if (piece.Owner == Player.Black && piece.GetType().Name == typeof(King).Name)
            {
                IsBlackKingMoved = true;
            }

            if (piece.Owner == Player.Black && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.A && move.Source.Rank == Rank.Eighth)
            {
                IsBlackQueenSideRookMoved = true;
            }

            if (piece.Owner == Player.Black && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.H && move.Source.Rank == Rank.Eighth)
            {
                IsBlackKingSideRookMoved = true;
            }


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

            Piece pieceSource = this[move.Source];
            Piece pieceDestination = this[move.Destination];
            return (WhoseTurn() == move.Player && pieceSource != null && pieceSource.Owner == move.Player &&
                    !Equals(move.Source, move.Destination) &&
                    (pieceDestination == null || pieceDestination.Owner != move.Player) &&
                    !ChessUtilities.PlayerWillBeInCheck(move, this) && pieceSource.IsValidGameMove(move, this));
        }

        internal static bool IsValidMove(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            Piece pieceSource = board[move.Source];
            Piece pieceDestination = board[move.Destination];

            return (pieceSource != null && pieceSource.Owner == move.Player &&
                    !Equals(move.Source, move.Destination) &&
                    (pieceDestination == null || pieceDestination.Owner != move.Player) &&
                    !ChessUtilities.PlayerWillBeInCheck(move, board) && pieceSource.IsValidGameMove(move, board));
        }

    }

    
}