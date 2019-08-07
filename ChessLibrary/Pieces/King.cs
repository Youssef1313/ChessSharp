using System;

namespace ChessLibrary.Pieces
{
    public class King : Piece
    {
        public King(Player player) : base(player) { }
        // TODO: Castling
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

            return move.GetAbsDeltaX() <= 1 && move.GetAbsDeltaY() <= 1;
        }
    }
}
