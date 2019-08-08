using System;
using ChessLibrary.Pieces;
using ChessLibrary.SquareData;
namespace ChessLibrary
{
    public class Move
    {
        public Square Source { get; }
        public Square Destination { get; }
        public Player Player { get; }
        public PawnPromotion? PromoteTo { get; }
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

        public Move(Square source, Square destination, Player player, PawnPromotion? promoteTo = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            Source = source;
            Destination = destination;
            Player = player;
            PromoteTo = promoteTo;
        }

        public byte GetAbsDeltaX()
        {
            return (byte) Math.Abs(Destination.File - Source.File);
        }

        public byte GetAbsDeltaY()
        {
            return (byte) Math.Abs(Destination.Rank - Source.Rank);
        }

        public sbyte GetDeltaX()
        {
            return (sbyte) (Destination.File - Source.File);
        }

        public sbyte GetDeltaY()
        {
            return (sbyte) (Destination.Rank - Source.Rank);
        }


    }
}