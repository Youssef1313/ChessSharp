using System;

namespace ChessLibrary.Pieces
{
    class Queen : Piece
    {
        protected override bool IsValidPieceMove(Move move)
        {
            return (move.GetAbsDeltaX() == 0 || move.GetAbsDeltaY() == 0) ||
                (move.GetAbsDeltaX() == move.GetAbsDeltaY());
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

            if (!IsValidPieceMove(move))
            {
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
