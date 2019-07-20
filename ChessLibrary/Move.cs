using System;
using ChessLibrary.PositionData;
namespace ChessLibrary
{
    public class Move
    {
        public Position Source { get; set; }
        public Position Destination { get; set; }
        public Player Player { get; set; }

        public Move(Position source, Position destination, Player player)
        {
            Source = source;
            Destination = destination;
            Player = player;
        }

        public byte GetAbsDeltaX()
        {
            return (byte)Math.Abs(Destination.File - Source.File);
        }

        public byte GetAbsDeltaY()
        {
            return (byte)Math.Abs(Destination.Rank - Source.Rank);
        }

        public sbyte GetDeltaX()
        {
            return (sbyte)(Destination.File - Source.File);
        }

        public sbyte GetDeltaY()
        {
            return (sbyte)(Destination.Rank - Source.Rank);
        }
    }
}