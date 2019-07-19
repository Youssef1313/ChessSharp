using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary.Pieces
{
    class King : Piece
    {
        protected override bool IsValidPieceMove(Move move)
        {
            return (move.GetAbsDeltaX() <= 1 && move.GetAbsDeltaY() <= 1);
        }

        public override bool IsValidGameMove(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (!IsValidPieceMove(move))
            {
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
