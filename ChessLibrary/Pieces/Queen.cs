using System;

namespace ChessLibrary.Pieces
{
    public class Queen : Piece
    {
        public override Player Owner { get; set; }

        protected override bool IsValidPieceMove(Move move)
        {
            int deltaX = move.GetAbsDeltaX();
            int deltaY = move.GetAbsDeltaY();
            if (deltaX == 0 && deltaY == 0)
            {
                return false;
            }
            return (deltaX == 0 || deltaY == 0) ||
                (deltaX == deltaY);
        }

        public override bool IsValidGameMove(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            return IsValidPieceMove(move) && !ChessUtilities.IsTherePieceInBetween(move, board);


        }
    }
}
