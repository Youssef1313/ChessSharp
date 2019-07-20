using System;
using ChessLibrary.PositionData;

namespace ChessLibrary.Pieces
{
    public class Bishop : Piece
    {
        public override Player Owner { get; set; }

        protected override bool IsValidPieceMove(Move move)
        {
            byte deltaX = move.GetAbsDeltaX();
            if (deltaX == 0)
            {
                return false;
            }
            return deltaX == move.GetAbsDeltaY();
        }

        public override bool IsValidGameMove(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            return IsValidPieceMove(move) && !ChessUtilities.PlayerWillBeInCheck(move, board.Board) &&
                   !ChessUtilities.IsTherePieceInBetween(move, board);

        }
    }
}