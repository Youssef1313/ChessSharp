using System;

namespace ChessLibrary.Pieces
{
    public class Knight : Piece
    {
        public override Player Owner { get; set; }

        protected override bool IsValidPieceMove(Move move)
        {
            byte deltaX = move.GetAbsDeltaX();
            byte deltaY = move.GetAbsDeltaY();
            return (deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1);
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

            if (ChessUtilities.PlayerWillBeInCheck(move, board))
            {
                return false;
            }

            if (board[move.Destination].Owner == move.Player)
            {
                return false; // Can't take your own piece.
            }
            throw new NotImplementedException();
        }
    }
}
