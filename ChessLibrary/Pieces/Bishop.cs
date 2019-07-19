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

            if (!IsValidPieceMove(move))
            {
                return false;
            }

            if (board[move.Destination].Owner == move.Player)
            {
                return false; // Can't take your own piece.
            }

            if (ChessUtilities.PlayerWillBeInCheck(move, board))
            {
                return false;
            }

            return !ChessUtilities.IsTherePieceInBetween(move, board);
        }
    }
}