using System;

namespace ChessLibrary.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player player) : base(player) { }

        protected override bool IsValidPieceMove(Move move)
        {
            byte deltaX = move.GetAbsDeltaX();
            byte deltaY = move.GetAbsDeltaY();
            return (deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1);
        }

        internal override bool IsValidGameMove(Move move, Piece[,] board)
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
