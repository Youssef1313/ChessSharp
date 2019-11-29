using System;

namespace ChessSharp.Pieces
{
    public class Rook : Piece
    {
        public Rook(Player player) : base(player) { }


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

            return (move.GetAbsDeltaX() == 0 || move.GetAbsDeltaY() == 0) && !ChessUtilities.IsTherePieceInBetween(move.Source, move.Destination, board.Board);

        }
    }
}
