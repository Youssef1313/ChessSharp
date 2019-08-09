using ChessLibrary.Pieces;
using ChessLibrary.SquareData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessLibrary
{

    public class GameBoard
    {
        public Piece this[File file, Rank rank] => Board[(int)rank, (int)file];
        public Piece this[Square square] => this[square.File, square.Rank];

        public List<Move> Moves { get; set; }
        public Piece[,] Board { get; set; }

        public GameState GameState { get; private set; }
        public bool CanWhiteCastleKingSide { get; private set; } = true;
        public bool CanWhiteCastleQueenSide { get; private set; } = true;
        public bool CanBlackCastleKingSide { get; private set; } = true;
        public bool CanBlackCastleQueenSide { get; private set; } = true;

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

        public static GameBoard Clone(GameBoard board)
        {
            return new GameBoard
            {
                Board = board.Board.Clone() as Piece[,],
                Moves = board.Moves,
                GameState =  board.GameState,
                CanBlackCastleKingSide =  board.CanBlackCastleKingSide,
                CanBlackCastleQueenSide = board.CanBlackCastleQueenSide,
                CanWhiteCastleKingSide = board.CanWhiteCastleKingSide,
                CanWhiteCastleQueenSide = board.CanWhiteCastleQueenSide
            };
        }
        public Player WhoseTurn()
        {
            return Moves.Count == 0 ? Player.White : ChessUtilities.RevertPlayer(Moves.Last().Player);
        }


        public bool MakeMove(Move move, bool isMoveValidated)
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

            if (!isMoveValidated && !IsValidMove(move))
            {
                return false;
            }

            SetCastleStatus(move, piece);

            if (piece.GetType().Name == typeof(Pawn).Name)
            {
                if ((move.Player == Player.White && move.Destination.Rank == Rank.Eighth) ||
                    (move.Player == Player.Black && move.Destination.Rank == Rank.First))
                {
                    switch (move.PromoteTo)
                    {
                        case PawnPromotion.Knight:
                            piece = new Knight(piece.Owner);
                            break;
                        case PawnPromotion.Bishop:
                            piece = new Bishop(piece.Owner);
                            break;
                        case PawnPromotion.Rook:
                            piece = new Rook(piece.Owner);
                            break;
                        case PawnPromotion.Queen:
                            piece = new Queen(piece.Owner);
                            break;
                        default:
                            throw new ArgumentNullException(nameof(move.PromoteTo));
                    }
                }
                // Enpassant
                if (Pawn.GetPawnMoveType(move) == PawnMoveType.Capture &&
                    this[move.Destination] == null)
                {
                    Board[(int) Moves.Last().Destination.Rank, (int) Moves.Last().Destination.File] = null;
                }

            }
            Board[(int) move.Source.Rank, (int) move.Source.File] = null;
            Board[(int) move.Destination.Rank, (int) move.Destination.File] = piece;
            Moves.Add(move);
            GameState = ChessUtilities.GetGameState(this);
            return true;
        }

        private void SetCastleStatus(Move move, Piece piece)
        {
            if (piece.Owner == Player.White && piece.GetType().Name == typeof(King).Name)
            {
                CanWhiteCastleKingSide = false;
                CanWhiteCastleQueenSide = false;
            }

            if (piece.Owner == Player.White && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.A && move.Source.Rank == Rank.First)
            {
                CanWhiteCastleQueenSide= false;
            }

            if (piece.Owner == Player.White && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.H && move.Source.Rank == Rank.First)
            {
                CanWhiteCastleKingSide = false;
            }

            if (piece.Owner == Player.Black && piece.GetType().Name == typeof(King).Name)
            {
                CanBlackCastleKingSide = false;
                CanBlackCastleQueenSide = false;
            }

            if (piece.Owner == Player.Black && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.A && move.Source.Rank == Rank.Eighth)
            {
                CanBlackCastleQueenSide= false;
            }

            if (piece.Owner == Player.Black && piece.GetType().Name == typeof(Rook).Name &&
                move.Source.File == File.H && move.Source.Rank == Rank.Eighth)
            {
                CanBlackCastleKingSide = false;
            }
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