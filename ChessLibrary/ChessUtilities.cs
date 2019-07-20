using System;
using System.Collections.Generic;
using ChessLibrary.PositionData;

namespace ChessLibrary
{
    public static class ChessUtilities
    {
        public static GameState GetGameState(GameBoard board)
        {
            throw new NotImplementedException();
        }

        public static List<Move> GetValidMoves(GameBoard board)
        {
            throw new NotImplementedException();
        }

        public static bool IsPlayerInCheck(Player player, GameBoard board)
        {
            throw new NotImplementedException();
        }

        public static bool PlayerWillBeInCheck(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }
            var boardClone = new GameBoard {Board = board.Board}; // Make the move on this board to keep original board as is.

            Piece piece = boardClone[move.Source];
            boardClone.Board[(int)move.Source.Rank, (int)move.Source.File] = null;
            boardClone.Board[(int)move.Destination.Rank, (int)move.Destination.File] = piece;
            return IsPlayerInCheck(move.Player, boardClone);
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
            var destination = new Position(move.Destination.File, move.Destination.Rank);
            while (true)
            {
                source.Rank += yStep;
                source.File += xStep;
                if (source.Rank == destination.Rank && source.File == destination.File)
                {
                    return false;
                }
                if (board[source] != null)
                {
                    return true;
                }
            }

        }
    }
}
