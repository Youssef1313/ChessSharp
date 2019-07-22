using System;

namespace ChessLibrary.Pieces
{
    public class Rook : Piece
    {
        public Rook(Player player) : base(player) { }

        protected override bool IsValidPieceMove(Move move)
        {
            return move.GetAbsDeltaX() == 0 || move.GetAbsDeltaY() == 0;
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

            return IsValidPieceMove(move) && !ChessUtilities.IsTherePieceInBetween(move, board);

        }
    }
}
