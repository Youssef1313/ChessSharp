namespace ChessSharp.Pieces
{
    /// <summary>Represents a queen <see cref="Piece"/>.</summary>
    public class Queen : Piece
    {
        internal Queen(Player player) : base(player) { }

        internal override bool IsValidGameMove(Move move, GameBoard board)
        {
            return new Rook(move.Player).IsValidGameMove(move, board) ||
                   new Bishop(move.Player).IsValidGameMove(move, board);
        }
    }
}
