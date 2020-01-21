using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;

namespace ChessSharp
{
    /// <summary>Represents a game move.</summary>
    public class Move : IDeepCloneable<Move>
    {
        /// <summary>Gets the source <see cref="Square"/> of the <see cref="Move"/>.</summary>
        public Square Source { get; }
        /// <summary>Gets the destination <see cref="Square"/> of the <see cref="Move"/>.</summary>
        public Square Destination { get; }
        /// <summary>Gets the <see cref="Player"/> of the <see cref="Move"/>.</summary>
        public Player Player { get; }
        /// <summary>Gets a nullable <see cref="PawnPromotion"/> of the <see cref="Move"/>.</summary>
        public PawnPromotion? PromoteTo { get; }

        public Move DeepClone()
        {
            return new Move(Source, Destination, Player, PromoteTo);
        }

        public override bool Equals(object obj)
        {

            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var moveObj = (Move) obj;
            return moveObj.Source == this.Source && moveObj.Destination == this.Destination;
        }

        public override int GetHashCode()
        {
            // Jon skeet's implementation:
            // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode

            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Source.GetHashCode();
                hash = hash * 23 + Destination.GetHashCode();
                hash = hash * 23 + Player.GetHashCode();
                return hash;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Move"/> class with the given arguments.</summary>
        /// <param name="source">The source <see cref="Square"/> of the <see cref="Move"/>.</param>
        /// <param name="destination">The destination <see cref="Square"/> of the <see cref="Move"/>.</param>
        /// <param name="player">The <see cref="Player"/> of the <see cref="Move"/>.</param>
        /// <param name="promoteTo">Optional nullable <see cref="PawnPromotion"/> of the <see cref="Move"/>. Default value is null.</param>
        public Move(Square source, Square destination, Player player, PawnPromotion? promoteTo = null)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Player = player;
            PromoteTo = promoteTo;
        }

        internal int GetAbsDeltaX()
        {
            return Math.Abs(Destination.File - Source.File);
        }

        internal int GetAbsDeltaY()
        {
            return Math.Abs(Destination.Rank - Source.Rank);
        }

        internal int GetDeltaX()
        {
            return Destination.File - Source.File;
        }

        internal int GetDeltaY()
        {
            return Destination.Rank - Source.Rank;
        }


    }
}