namespace ChessSharp
{
    /// <summary>Represents the base class of the pieces.</summary>
    public abstract class Piece
    {

        /// <summary>Gets the owner <see cref="Player"/> of the piece.</summary>
        public Player Owner { get; }

        internal abstract bool IsValidGameMove(Move move, GameBoard board);

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            return Owner == ((Piece) obj).Owner;
        }

        public override int GetHashCode()
        {
            // Jon skeet's implementation:
            // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode

            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + this.GetType().Name.GetHashCode();
                hash = hash * 23 + Owner.GetHashCode();
                return hash;
            }
        }

        protected Piece(Player player)
        {
            Owner = player;
        }

    }
}
