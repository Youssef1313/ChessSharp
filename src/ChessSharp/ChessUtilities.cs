using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessSharp
{
    /// <summary>
    /// A static class containing helper methods.
    /// </summary>
    public static class ChessUtilities
    {
        private static readonly IEnumerable<Square> s_allSquares =
            from file in Enum.GetValues(typeof(File)).Cast<File>()
            from rank in Enum.GetValues(typeof(Rank)).Cast<Rank>()
            select new Square(file, rank);

        internal static Player RevertPlayer(Player player) => player == Player.White ? Player.Black : Player.White;

        internal static GameState GetGameState(GameBoard board)
        {
            Player opponent = board.WhoseTurn();
            Player lastPlayer = RevertPlayer(opponent);
            bool isInCheck = IsPlayerInCheck(opponent, board);
            var hasValidMoves = GetValidMoves(board).Count > 0;

            if (isInCheck && !hasValidMoves)
            {
                return lastPlayer == Player.White ? GameState.WhiteWinner : GameState.BlackWinner;
            }

            if (!hasValidMoves)
            {
                return GameState.Stalemate;
            }

            if (isInCheck)
            {
                return opponent == Player.White ? GameState.WhiteInCheck : GameState.BlackInCheck;
            }

            return board.IsInsufficientMaterial() ? GameState.Draw : GameState.NotCompleted;
        }

        /* TODO: Still not sure where to implement it, but I may need methods:
           TODO: bool CanClaimDraw + bool ClaimDraw + OfferDraw
        */

        /// <summary>Gets the valid moves of the given <see cref="GameBoard"/>.</summary>
        /// <param name="board">The <see cref="GameBoard"/> that you want to get its valid moves.</param>
        /// <returns>Returns a list of the valid moves.</returns>
        public static List<Move> GetValidMoves(GameBoard board)
        {
            Player player = board.WhoseTurn();
            var validMoves = new List<Move>();

            IEnumerable<Square> playerOwnedSquares = s_allSquares.Where(sq => board[sq]?.Owner == player);
            Square[] nonPlayerOwnedSquares = s_allSquares.Where(sq => board[sq]?.Owner != player).ToArray(); // Converting to array to avoid "Possible multiple enumeration" as suggested by ReSharper.

            foreach (Square playerOwnedSquare in playerOwnedSquares)
            {
                validMoves.AddRange(nonPlayerOwnedSquares
                    .Select(nonPlayerOwnedSquare => new Move(playerOwnedSquare, nonPlayerOwnedSquare, player))
                    .Where(move => GameBoard.IsValidMove(move, board)));
            }

            return validMoves;
        }

        /// <summary>Gets the valid moves of the given <see cref="GameBoard"/> that has a specific given source <see cref="Square"/>.</summary>
        /// <param name="source">The source <see cref="Square"/> that you're looking for its valid moves.</param>
        /// <param name="board">The <see cref="GameBoard"/> that you want to get its valid moves from the specified square.</param>
        /// <returns>Returns a list of the valid moves that has the given source square.</returns>
        /// 
        public static List<Move> GetValidMovesOfSourceSquare(Square source, GameBoard board)
        {
            var validMoves = new List<Move>();
            Piece piece = board[source];
            if (piece == null || piece.Owner != board.WhoseTurn())
            {
                return validMoves;
            }

            Player player = piece.Owner;
            Square[] nonPlayerOwnedSquares = s_allSquares.Where(sq => board[sq]?.Owner != player).ToArray();

            validMoves.AddRange(nonPlayerOwnedSquares
                .Select(nonPlayerOwnedSquare => new Move(source, nonPlayerOwnedSquare, player, PawnPromotion.Queen)) // If promoteTo is null, valid pawn promotion will cause exception. Need to implement this better and cleaner in the future.
                .Where(move => GameBoard.IsValidMove(move, board)));
            return validMoves;
        }

        internal static bool IsPlayerInCheck(Player player, GameBoard board)
        {
            IEnumerable<Square> opponentOwnedSquares = s_allSquares.Where(sq => board[sq]?.Owner == RevertPlayer(player));
            Square playerKingSquare = s_allSquares.First(sq => new King(player).Equals(board[sq]));

            return (from opponentOwnedSquare in opponentOwnedSquares
                    let piece = board[opponentOwnedSquare]
                    let move = new Move(opponentOwnedSquare, playerKingSquare, RevertPlayer(player), PawnPromotion.Queen) // Added PawnPromotion in the Move because omitting it causes a bug when King in its rank is in a check by a pawn.
                    where piece.IsValidGameMove(move, board)
                    select piece).Any();
        }

        internal static bool PlayerWillBeInCheck(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            GameBoard boardClone = board.DeepClone(); // Make the move on this board to keep original board as is.
            Piece piece = boardClone[move.Source];
            boardClone.Board[(int)move.Source.Rank, (int)move.Source.File] = null;
            boardClone.Board[(int)move.Destination.Rank, (int)move.Destination.File] = piece;

            return IsPlayerInCheck(move.Player, boardClone);
        }

        internal static bool IsTherePieceInBetween(Square square1, Square square2, Piece[,] board)
        {
            int xStep = Math.Sign(square2.File - square1.File);
            int yStep = Math.Sign(square2.Rank - square1.Rank);

            Rank rank = square1.Rank;
            File file = square1.File;
            while (true) // TODO: Prevent possible infinite loop (by throwing an exception) when passing un-logical squares (two squares not on same file, rank, or diagonal).
            {
                rank += yStep;
                file += xStep;
                if (rank == square2.Rank && file == square2.File)
                {
                    return false;
                }

                if (board[(int)rank, (int)file] != null)
                {
                    return true;
                }
            }

        }
    }
}
