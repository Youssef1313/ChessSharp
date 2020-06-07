using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessSharp
{
    /// <summary>A static class containing helper methods.</summary>
    public static class ChessUtilities
    {
        private static readonly IEnumerable<Square> s_allSquares =
            from file in Enum.GetValues(typeof(File)).Cast<File>()
            from rank in Enum.GetValues(typeof(Rank)).Cast<Rank>()
            select new Square(file, rank);

        internal static Player RevertPlayer(Player player) => player == Player.White ? Player.Black : Player.White;

        /* TODO: Still not sure where to implement it, but I may need methods:
           TODO: bool CanClaimDraw + bool ClaimDraw + OfferDraw
        */

        /// <summary>Gets the valid moves of the given <see cref="ChessGame"/>.</summary>
        /// <param name="board">The <see cref="ChessGame"/> that you want to get its valid moves.</param>
        /// <returns>Returns a list of the valid moves.</returns>
        public static List<Move> GetValidMoves(ChessGame board)
        {
            // Although nullable is enabled and board is non-nullable ref type, this check is needed
            // because this is a public method that can be used by an application that doesn't have
            // nullable enabled.
            _ = board ?? throw new ArgumentNullException(nameof(board));
            

            Player player = board.WhoseTurn;
            var validMoves = new List<Move>();

            IEnumerable<Square> playerOwnedSquares = s_allSquares.Where(sq => board[sq.File, sq.Rank]?.Owner == player);
            Square[] nonPlayerOwnedSquares = s_allSquares.Where(sq => board[sq.File, sq.Rank]?.Owner != player).ToArray(); // Converting to array to avoid "Possible multiple enumeration" as suggested by ReSharper.

            foreach (Square playerOwnedSquare in playerOwnedSquares)
            {
                validMoves.AddRange(nonPlayerOwnedSquares
                    .Select(nonPlayerOwnedSquare => new Move(playerOwnedSquare, nonPlayerOwnedSquare, player))
                    .Where(move => ChessGame.IsValidMove(move, board)));
            }

            return validMoves;
        }

        /// <summary>Gets the valid moves of the given <see cref="ChessGame"/> that has a specific given source <see cref="Square"/>.</summary>
        /// <param name="source">The source <see cref="Square"/> that you're looking for its valid moves.</param>
        /// <param name="board">The <see cref="ChessGame"/> that you want to get its valid moves from the specified square.</param>
        /// <returns>Returns a list of the valid moves that has the given source square.</returns>
        /// 
        public static List<Move> GetValidMovesOfSourceSquare(Square source, ChessGame board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            var validMoves = new List<Move>();
            Piece? piece = board[source.File, source.Rank];
            if (piece == null || piece.Owner != board.WhoseTurn)
            {
                return validMoves;
            }

            Player player = piece.Owner;
            Square[] nonPlayerOwnedSquares = s_allSquares.Where(sq => board[sq.File, sq.Rank]?.Owner != player).ToArray();

            validMoves.AddRange(nonPlayerOwnedSquares
                .Select(nonPlayerOwnedSquare => new Move(source, nonPlayerOwnedSquare, player, PawnPromotion.Queen)) // If promoteTo is null, valid pawn promotion will cause exception. Need to implement this better and cleaner in the future.
                .Where(move => ChessGame.IsValidMove(move, board)));
            return validMoves;
        }

        internal static bool IsPlayerInCheck(Player player, ChessGame board)
        {
            Player opponent = RevertPlayer(player);
            IEnumerable<Square> opponentOwnedSquares = s_allSquares.Where(sq => board[sq.File, sq.Rank]?.Owner == opponent);
            Square playerKingSquare = s_allSquares.First(sq => new King(player).Equals(board[sq.File, sq.Rank]));

            return (from opponentOwnedSquare in opponentOwnedSquares
                    let piece = board[opponentOwnedSquare.File, opponentOwnedSquare.Rank]
                    let move = new Move(opponentOwnedSquare, playerKingSquare, opponent, PawnPromotion.Queen) // Added PawnPromotion in the Move because omitting it causes a bug when King in its rank is in a check by a pawn.
                    where piece.IsValidGameMove(move, board)
                    select piece).Any();
        }

    }
}
