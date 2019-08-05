using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using ChessLibrary.Pieces;
using ChessLibrary.SquareData;

namespace ChessLibrary
{
    public static class ChessUtilities
    {
        public static Player RevertPlayer(Player player)
        {
            return player == Player.White ? Player.Black : Player.White;
        }

        public static GameState GetGameState(Piece[,] board, Player lastPlayer)
        {
            throw new NotImplementedException();
        }

        internal static List<Move> GetValidMoves(Piece[,] board, Player lastPlayer)
        {
            throw new NotImplementedException();
        }

        internal static bool IsPlayerInCheck(Player player, Piece[,] board)
        {
            Square[] squares = new[]
            {
                "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8",
                "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8",
                "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8",
                "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8",
                "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8",
                "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8",
                "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8",
                "H1", "H2", "H3", "H4", "H5", "H6", "H7", "H8",
            }.Select(Square.Parse).ToArray();
       
            var opponentOwnedSquares = squares.Where(sq => board[(int)sq.Rank, (int)sq.File] != null &&
                                                           board[(int)sq.Rank, (int)sq.File].Owner != player);
            var playerKingSquare = squares.First(sq => board[(int) sq.Rank, (int) sq.File].Equals(new King(player)));

            return (from opponentOwnedSquare in opponentOwnedSquares
                    let piece = board[(int) opponentOwnedSquare.Rank, (int) opponentOwnedSquare.File]
                    let move = new Move(opponentOwnedSquare, playerKingSquare, RevertPlayer(player))
                    where piece.IsValidGameMove(move, board)
                    select piece).Any();
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
