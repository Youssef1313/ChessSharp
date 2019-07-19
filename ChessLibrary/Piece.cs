namespace ChessLibrary
{
    public abstract class Piece
    {
        protected abstract bool IsValidPieceMove(Move move);
        public abstract bool IsValidGameMove(Move move, GameBoard board);
    }
}
