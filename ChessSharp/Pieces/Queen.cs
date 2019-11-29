namespace ChessSharp.Pieces
{
    public class Queen : Piece
    {
        public Queen(Player player) : base(player) { }

        internal override bool IsValidGameMove(Move move, GameBoard board)
        {
            return new Rook(move.Player).IsValidGameMove(move, board) ||
                   new Bishop(move.Player).IsValidGameMove(move, board);
        }
    }
}
