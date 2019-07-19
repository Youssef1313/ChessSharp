using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
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
            return (sbyte)Math.Abs(Destination.File - Source.File);
        }

        public sbyte GetDeltaY()
        {
            return (sbyte)Math.Abs(Destination.Rank - Source.Rank);
        }
    }
}