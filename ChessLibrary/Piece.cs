using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public abstract class Piece
    {
        protected abstract bool IsValidPieceMove(Move move);
        public abstract bool IsValidGameMove(Move move, GameBoard board);
    }
}
