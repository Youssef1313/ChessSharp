using System;

namespace ChessLibrary.Pieces
{
    public class Queen : Piece
    {
        public override Player Owner { get; set; }

        protected override bool IsValidPieceMove(Move move)
        {
            // This method is not needed for this sub-class.
            throw new NotImplementedException();
        }

        internal override bool IsValidGameMove(Move move, GameBoard board)
        {
            return new Rook {Owner = move.Player}.IsValidGameMove(move, board) ||
                   new Bishop { Owner = move.Player}.IsValidGameMove(move, board);
        }
    }
}
