using System;
using ChessLibrary.PositionData;

namespace ChessLibrary.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Player player) : base(player) { }

        protected override bool IsValidPieceMove(Move move)
        {
            return move.GetAbsDeltaX() == move.GetAbsDeltaY();
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

            return IsValidPieceMove(move) && !ChessUtilities.IsTherePieceInBetween(move, board);
        }
    }
}