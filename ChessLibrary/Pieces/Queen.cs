using System;

namespace ChessLibrary.Pieces
{
    public class Queen : Piece
    {
        public Queen(Player player) : base(player) { }

        protected override bool IsValidPieceMove(Move move)
        {
            // This method is not needed for this sub-class.
            throw new NotImplementedException();
        }

        internal override bool IsValidGameMove(Move move, GameBoard board)
        {
            return new Rook(move.Player).IsValidGameMove(move, board) ||
                   new Bishop(move.Player).IsValidGameMove(move, board);
        }
    }
}
