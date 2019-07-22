namespace ChessLibrary
{
    public abstract class Piece
    {
        protected abstract bool IsValidPieceMove(Move move);
        internal abstract bool IsValidGameMove(Move move, GameBoard board);
        public abstract Player Owner { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            return Owner == ((Piece) obj).Owner;
        }

    }
}
