using System;

namespace ChessLibrary.Pieces
{
    public class Rook : Piece
    {
        public Rook(Player player) : base(player) { }


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

            return (move.GetAbsDeltaX() == 0 || move.GetAbsDeltaY() == 0) && !ChessUtilities.IsTherePieceInBetween(move, board);

        }
    }
}
