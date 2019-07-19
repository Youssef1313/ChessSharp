using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public static class ChessUtilities
    {
        public static bool IsPlayerInCheck(Player player, GameBoard board)
        {
            throw new NotImplementedException();
        }

        public static bool PlayerWillBeInCheck(Move move, GameBoard board)
        {
            // TODO: Make the move.
            throw new NotImplementedException();
            return IsPlayerInCheck(move.Player, board);
        }
    }
}
