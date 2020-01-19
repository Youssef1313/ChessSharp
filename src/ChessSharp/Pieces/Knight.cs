using System;

namespace ChessSharp.Pieces
{
    /// <summary>Represents a knight <see cref="Piece"/>.</summary>
    public class Knight : Piece
    {
        internal Knight(Player player) : base(player) { }

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
            int deltaX = move.GetAbsDeltaX();
            int deltaY = move.GetAbsDeltaY();
            return (deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1);
        }
    }
}
