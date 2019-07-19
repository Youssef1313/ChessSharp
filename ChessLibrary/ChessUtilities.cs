using System;
using ChessLibrary.PositionData;

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

        public static bool IsTherePieceInBetween(Move move, GameBoard board)
        {
            var xStep = 0;
            var yStep = 0;

            if (move.Destination.File > move.Source.File)
            {
                xStep = 1;
            }
            if (move.Destination.Rank > move.Source.Rank)
            {
                yStep = 1;
            }

            if (move.Destination.File < move.Source.File)
            {
                xStep = -1;
            }
            if (move.Destination.Rank < move.Source.Rank)
            {
                yStep = -1;
            }

            var source = new Position(move.Source.File, move.Source.Rank);
            var destination = new Position(move.Source.File, move.Source.Rank);
            while (source.Rank != destination.Rank)
            {
                source.Rank += yStep;
                source.File += xStep;
                if (board[source] != null)
                {
                    return true;
                }
            }

            return false;

        }
    }
}
