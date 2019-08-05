using System;
using System.Collections.Generic;
using System.Linq;
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
            var squares = new Square[]
            {
                Square.ParseSquare("A1"), Square.ParseSquare("A2"), Square.ParseSquare("A3"), Square.ParseSquare("A4"),
                Square.ParseSquare("A5"), Square.ParseSquare("A6"), Square.ParseSquare("A7"), Square.ParseSquare("A8"),

                Square.ParseSquare("B1"), Square.ParseSquare("B2"), Square.ParseSquare("B3"), Square.ParseSquare("B4"),
                Square.ParseSquare("B5"), Square.ParseSquare("B6"), Square.ParseSquare("B7"), Square.ParseSquare("B8"),

                Square.ParseSquare("C1"), Square.ParseSquare("C2"), Square.ParseSquare("C3"), Square.ParseSquare("C4"),
                Square.ParseSquare("C5"), Square.ParseSquare("C6"), Square.ParseSquare("C7"), Square.ParseSquare("C8"),

                Square.ParseSquare("D1"), Square.ParseSquare("D2"), Square.ParseSquare("D3"), Square.ParseSquare("D4"),
                Square.ParseSquare("D5"), Square.ParseSquare("D6"), Square.ParseSquare("D7"), Square.ParseSquare("D8"),

                Square.ParseSquare("E1"), Square.ParseSquare("E2"), Square.ParseSquare("E3"), Square.ParseSquare("E4"),
                Square.ParseSquare("E5"), Square.ParseSquare("E6"), Square.ParseSquare("E7"), Square.ParseSquare("E8"),

                Square.ParseSquare("F1"), Square.ParseSquare("F2"), Square.ParseSquare("F3"), Square.ParseSquare("F4"),
                Square.ParseSquare("F5"), Square.ParseSquare("F6"), Square.ParseSquare("F7"), Square.ParseSquare("F8"),

                Square.ParseSquare("G1"), Square.ParseSquare("G2"), Square.ParseSquare("G3"), Square.ParseSquare("G4"),
                Square.ParseSquare("G5"), Square.ParseSquare("G6"), Square.ParseSquare("G7"), Square.ParseSquare("G8"),

                Square.ParseSquare("H1"), Square.ParseSquare("H2"), Square.ParseSquare("H3"), Square.ParseSquare("H4"),
                Square.ParseSquare("H5"), Square.ParseSquare("H6"), Square.ParseSquare("H7"), Square.ParseSquare("H8"),
            };
            var opponentOwnedSquares = squares.Where(sq => board[(int)sq.Rank, (int)sq.File] != null &&
                                                           board[(int)sq.Rank, (int)sq.File].Owner != player);
            var playerKingSquare = squares.First(sq => board[(int) sq.Rank, (int) sq.File].GetHashCode() == new King(player).GetHashCode());

            return (from opponentOwnedSquare in opponentOwnedSquares
                    let piece = board[(int) opponentOwnedSquare.Rank, (int) opponentOwnedSquare.File]
                    let move = new Move(opponentOwnedSquare, playerKingSquare, ChessUtilities.RevertPlayer(player))
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
