using System;

namespace ChessSharp.Pieces
{
    /// <summary>Represents a rook <see cref="Piece"/>.</summary>
    public class Rook : Piece
    {
        internal Rook(Player player) : base(player) { }


        internal override bool IsValidGameMove(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            return (move.GetAbsDeltaX() == 0 || move.GetAbsDeltaY() == 0) && !board.IsTherePieceInBetween(move.Source, move.Destination);

        }
    }
}
