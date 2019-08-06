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
            Player opponent = RevertPlayer(lastPlayer);
            bool hasValidMoves = GetValidMoves(board, opponent).Count > 0;
            bool isInCheck = IsPlayerInCheck(opponent, board);

            if (isInCheck && !hasValidMoves)
            {
                return lastPlayer == Player.White ? GameState.WhiteWinner : GameState.BlackWinner;
            }

            if (!hasValidMoves)
            {
                return GameState.Stalemate;
            }

            return IsInsufficientMaterial(board) ? GameState.Draw : GameState.NotCompleted;
        }

        /* TODO: Still not sure where to implement it, but I may need methods:
           TODO: bool CanClaimDraw + bool ClaimDraw + OfferDraw
        */

        internal static bool IsInsufficientMaterial(Piece[,] board)
        {
            Piece[] pieces = board.Cast<Piece>().ToArray();
            var whitePieces = pieces.Where(p => p.Owner == Player.White).ToArray();
            var blackPieces = pieces.Where(p => p.Owner == Player.Black).ToArray();
            if (whitePieces.Length == 1 && blackPieces.Length == 1) // King vs King
            {
                return true;
            }


            if (whitePieces.Length == 1 && blackPieces.Length == 2 &&
                blackPieces.Any(p => p.GetType().Name == typeof(Bishop).Name ||
                                     p.GetType().Name == typeof(Knight).Name)) // White King vs black king and (Bishop|Knight)
            {
                return true;
            }

            if (whitePieces.Length == 2 && blackPieces.Length == 1 &&
                whitePieces.Any(p => p.GetType().Name == typeof(Bishop).Name ||
                                     p.GetType().Name == typeof(Knight).Name)) // Black King vs white king and (Bishop|Knight)
            {
                return true;
            }

            if (whitePieces.Length == 2 && blackPieces.Length == 2 &&
                whitePieces.Any(p => p.GetType().Name == typeof(Bishop).Name) &&
                blackPieces.Any(p => p.GetType().Name == typeof(Bishop).Name)) // King and bishop vs king and bishop
            {
                // TODO: Bug here, should check that bishops are of same color square
                return true;
            }
            return false;
        }

        internal static List<Move> GetValidMoves(Piece[,] board, Player player)
        {
            var validMoves = new List<Move>();
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

            var playerOwnedSquares = squares.Where(sq => board[(int)sq.Rank, (int)sq.File] != null &&
                                                         board[(int)sq.Rank, (int)sq.File].Owner == player).ToArray();
            var nonPlayerOwnedSquares = squares.Where(sq => board[(int)sq.Rank, (int)sq.File] == null ||
                                                            board[(int)sq.Rank, (int)sq.File].Owner != player).ToArray();

            foreach (var playerOwnedSquare in playerOwnedSquares)
            {
                validMoves.AddRange(from nonPlayerOwnedSquare in nonPlayerOwnedSquares
                                    select new Move(playerOwnedSquare, nonPlayerOwnedSquare, player) into move
                                    let piece = board[(int) playerOwnedSquare.Rank, (int) playerOwnedSquare.File]
                                    where piece.IsValidGameMove(move, board)
                                    select move);
            }

            return validMoves;
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
            var playerKingSquare = squares.First(sq => new King(player).Equals(board[(int)sq.Rank, (int)sq.File]));

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
