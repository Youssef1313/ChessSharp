using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary.Pieces
{
    class Knight : Piece
    {
        protected override bool IsValidPieceMove(Move move)
        {
            byte deltaX = move.GetAbsDeltaX();
            byte deltaY = move.GetAbsDeltaY();
            return (deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1);
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
