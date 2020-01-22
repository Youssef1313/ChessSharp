using System;

namespace ChessSharp.Pieces
{
    /// <summary>Represents a bishop <see cref="Piece"/>.</summary>
    public class Bishop : Piece
    {
        internal Bishop(Player player) : base(player) { }


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

            return move.GetAbsDeltaX() == move.GetAbsDeltaY() && !board.IsTherePieceInBetween(move.Source, move.Destination);
        }
    }
}