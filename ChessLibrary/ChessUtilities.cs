using System;
using System.Collections.Generic;
using ChessLibrary.SquareData;

namespace ChessLibrary
{
    public static class ChessUtilities
    {
        public static Player RevertPlayer(Player player)
        {
            return player == Player.White ? Player.Black : Player.White;
        }

        public static GameState GetGameState(Piece[,] board)
        {
            throw new NotImplementedException();
        }

        internal static List<Move> GetValidMoves(Piece[,] board)
        {
            throw new NotImplementedException();
        }

        internal static bool IsPlayerInCheck(Player player, Piece[,] board)
        {
            return false;
            throw new NotImplementedException();
        }

        internal static bool PlayerWillBeInCheck(Move move, Piece[,] board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }
            
            var boardClone = board.Clone() as Piece[,]; // Make the move on this board to keep original board as is.
            Piece piece = boardClone[(int)move.Source.Rank, (int)move.Source.File];
            boardClone[(int)move.Source.Rank, (int)move.Source.File] = null;
            boardClone[(int)move.Destination.Rank, (int)move.Destination.File] = piece;
            return IsPlayerInCheck(move.Player, boardClone);
        }

        internal static bool IsTherePieceInBetween(Move move, Piece[,] board)
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

            var source = new Square(move.Source.File, move.Source.Rank);
            var destination = new Square(move.Destination.File, move.Destination.Rank);
            Rank rank = source.Rank;
            File file = source.File;
            while (true)
            {
                rank += yStep;
                file += xStep;
                if (rank == destination.Rank && file == destination.File)
                {
                    return false;
                }
                
                if (board[(int)rank, (int)file] != null)
                {
                    return true;
                }
            }

        }
    }
}
