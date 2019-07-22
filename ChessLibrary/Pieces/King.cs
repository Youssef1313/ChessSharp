using System;

namespace ChessLibrary.Pieces
{
    public class King : Piece
    {
        public King(Player player) : base(player) { }


        protected override bool IsValidPieceMove(Move move)
        {
            return move.GetAbsDeltaX() <= 1 && move.GetAbsDeltaY() <= 1;
        }

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

            return IsValidPieceMove(move);
        }
    }
}
