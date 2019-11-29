using System;

namespace ChessSharp.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Player player) : base(player) { }


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

            return move.GetAbsDeltaX() == move.GetAbsDeltaY() && !ChessUtilities.IsTherePieceInBetween(move.Source, move.Destination, board.Board);
        }
    }
}