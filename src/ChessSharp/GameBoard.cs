using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessSharp
{
    /// <summary>Represents the chess game.</summary>
    public class GameBoard : IDeepCloneable<GameBoard>
    {
        /// <summary>Gets <see cref="Piece"/> in a specific square.</summary>
        /// <param name="file">The <see cref="File"/> of the square.</param>
        /// <param name="rank">The <see cref="Rank"/> of the square.</param>
        public Piece this[File file, Rank rank] => Board[(int)rank, (int)file];

        /// <summary>Gets <see cref="Piece"/> in a specific square.</summary>
        /// <param name="square">The <see cref="Square"/> to get its <see cref="Piece"/>.</param>
        public Piece this[Square square] => this[square.File, square.Rank];

        /// <summary>Gets a list of the game moves.</summary>
        public List<Move> Moves { get; private set; }

        /// <summary>Gets a 2D array of <see cref="Piece"/>s in the board.</summary>
        public Piece[,] Board { get; private set; } // TODO: It's bad idea to expose this to public.

        /// <summary>Gets the current <see cref="ChessSharp.GameState"/>.</summary>
        public GameState GameState { get; private set; }


        internal bool CanWhiteCastleKingSide { get; set; } = true;
        internal bool CanWhiteCastleQueenSide { get; set; } = true;
        internal bool CanBlackCastleKingSide { get; set; } = true;
        internal bool CanBlackCastleQueenSide { get; set; } = true;

        /// <summary>Initializes a new instance of <see cref="GameBoard"/>.</summary>
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

        /// <summary>Gets the <see cref="Player"/> who has turn.</summary>
        public Player WhoseTurn()
        {
            return Moves.Count == 0 ? Player.White : ChessUtilities.RevertPlayer(Moves.Last().Player);
        }

        /// <summary>Makes a move in the game.</summary>
        /// <param name="move">The <see cref="Move"/> you want to make.</param>
        /// <param name="isMoveValidated">Only pass true when you've already checked that the move is valid.</param>
        /// <returns>Returns true if the move is made; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///     The <c>move</c> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="Move.Source"/> square of the <c>move</c> doesn't contain a piece.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///    The <c>move.PromoteTo</c> is null and the move is a pawn promotion move.
        /// </exception>
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

            if (piece is King && move.GetAbsDeltaX() == 2)
            {
                // Queen-side castle
                if (move.Destination.File == File.C)
                {
                    var rook = this[File.A, move.Source.Rank];
                    Board[(int)move.Source.Rank, (int)File.A] = null;
                    Board[(int)move.Source.Rank, (int)File.D] = rook;
                }

                // King-side castle
                if (move.Destination.File == File.G)
                {
                    var rook = this[File.H, move.Source.Rank];
                    Board[(int)move.Source.Rank, (int)File.H] = null;
                    Board[(int)move.Source.Rank, (int)File.F] = rook;
                }
            }

            if (piece is Pawn)
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
            SetGameState();
            return true;
        }

        private void SetCastleStatus(Move move, Piece piece)
        {
            if (piece.Owner == Player.White && piece is King)
            {
                CanWhiteCastleKingSide = false;
                CanWhiteCastleQueenSide = false;
            }

            if (piece.Owner == Player.White && piece is Rook &&
                move.Source.File == File.A && move.Source.Rank == Rank.First)
            {
                CanWhiteCastleQueenSide= false;
            }

            if (piece.Owner == Player.White && piece is Rook &&
                move.Source.File == File.H && move.Source.Rank == Rank.First)
            {
                CanWhiteCastleKingSide = false;
            }

            if (piece.Owner == Player.Black && piece is King)
            {
                CanBlackCastleKingSide = false;
                CanBlackCastleQueenSide = false;
            }

            if (piece.Owner == Player.Black && piece is Rook &&
                move.Source.File == File.A && move.Source.Rank == Rank.Eighth)
            {
                CanBlackCastleQueenSide= false;
            }

            if (piece.Owner == Player.Black && piece is Rook &&
                move.Source.File == File.H && move.Source.Rank == Rank.Eighth)
            {
                CanBlackCastleKingSide = false;
            }
        }

        /// <summary>Checks if a given move is valid or not.</summary>
        /// <param name="move">The <see cref="Move"/> to check its validity.</param>
        /// <returns>Returns true if the given <c>move</c> is valid; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///     The given <c>move</c> is null.
        /// </exception>
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
                    !PlayerWillBeInCheck(move) && pieceSource.IsValidGameMove(move, this));
        }

        internal bool PlayerWillBeInCheck(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            GameBoard boardClone = DeepClone(); // Make the move on this board to keep original board as is.
            Piece piece = boardClone[move.Source];
            boardClone.Board[(int)move.Source.Rank, (int)move.Source.File] = null;
            boardClone.Board[(int)move.Destination.Rank, (int)move.Destination.File] = piece;

            return ChessUtilities.IsPlayerInCheck(move.Player, boardClone);
        }

        internal void SetGameState()
        {
            Player opponent = this.WhoseTurn();
            Player lastPlayer = ChessUtilities.RevertPlayer(opponent);
            bool isInCheck = ChessUtilities.IsPlayerInCheck(opponent, this);
            var hasValidMoves = ChessUtilities.GetValidMoves(this).Count > 0;

            if (isInCheck && !hasValidMoves)
            {
                GameState = lastPlayer == Player.White ? GameState.WhiteWinner : GameState.BlackWinner;
                return;
            }

            if (!hasValidMoves)
            {
                GameState = GameState.Stalemate;
                return;
            }

            if (isInCheck)
            {
                GameState = opponent == Player.White ? GameState.WhiteInCheck : GameState.BlackInCheck;
                return;
            }
            GameState = IsInsufficientMaterial() ? GameState.Draw : GameState.NotCompleted;
        }

        internal bool IsInsufficientMaterial()
        {
            Piece[] pieces = Board.Cast<Piece>().ToArray();

            var whitePieces = pieces.Select((p, i) => new { Piece = p, SquareColor = (i % 8 + i / 8) % 2 })
                .Where(p => p.Piece?.Owner == Player.White).ToArray();

            var blackPieces = pieces.Select((p, i) => new { Piece = p, SquareColor = (i % 8 + i / 8) % 2 })
                .Where(p => p.Piece?.Owner == Player.Black).ToArray();

            switch (whitePieces.Length)
            {
                // King vs King
                case 1 when blackPieces.Length == 1:
                // White King vs black king and (Bishop|Knight)
                case 1 when blackPieces.Length == 2 && blackPieces.Any(p => p.Piece is Bishop ||
                                                                            p.Piece is Knight):
                // Black King vs white king and (Bishop|Knight)
                case 2 when blackPieces.Length == 1 && whitePieces.Any(p => p.Piece is Bishop ||
                                                                            p.Piece is Knight):
                    return true;
                // King and bishop vs king and bishop
                case 2 when blackPieces.Length == 2:
                {
                    var whiteBishop = whitePieces.First(p => p.Piece is Bishop);
                    var blackBishop = blackPieces.First(p => p.Piece is Bishop);
                    return whiteBishop != null && blackBishop != null &&
                           whiteBishop.SquareColor == blackBishop.SquareColor;
                }
                default:
                    return false;
            }
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
                    !board.PlayerWillBeInCheck(move) && pieceSource.IsValidGameMove(move, board));
        }

        public GameBoard DeepClone()
        {
            return new GameBoard
            {
                Board = Board.Clone() as Piece[,],
                Moves = Moves.Select(m => m.DeepClone()).ToList(),
                GameState = GameState,
                CanBlackCastleKingSide = CanBlackCastleKingSide,
                CanBlackCastleQueenSide = CanBlackCastleQueenSide,
                CanWhiteCastleKingSide = CanWhiteCastleKingSide,
                CanWhiteCastleQueenSide = CanWhiteCastleQueenSide
            };
        }
    }
}