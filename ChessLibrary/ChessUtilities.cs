using ChessLibrary.Pieces;
using ChessLibrary.SquareData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessLibrary
{
    public static class ChessUtilities
    {
        public static Player RevertPlayer(Player player)
        {
            return player == Player.White ? Player.Black : Player.White;
        }

        internal static GameState GetGameState(GameBoard board)
        {
            Player opponent = board.WhoseTurn();
            Player lastPlayer = RevertPlayer(opponent);
            bool hasValidMoves = GetValidMoves(board).Count > 0;
            bool isInCheck = IsPlayerInCheck(opponent, board);

            if (isInCheck && !hasValidMoves)
            {
                return lastPlayer == Player.White ? GameState.WhiteWinner : GameState.BlackWinner;
            }

            if (!hasValidMoves)
            {
                return GameState.Stalemate;
            }

            if (isInCheck)
            {
                return opponent == Player.White ? GameState.WhiteInCheck : GameState.BlackInCheck;
            }

            return IsInsufficientMaterial(board.Board) ? GameState.Draw : GameState.NotCompleted;
        }

        /* TODO: Still not sure where to implement it, but I may need methods:
           TODO: bool CanClaimDraw + bool ClaimDraw + OfferDraw
        */

        internal static bool IsInsufficientMaterial(Piece[,] board)
        {
            Piece[] pieces = board.Cast<Piece>().ToArray();
            var whitePieces = pieces.Select((p, i) => new { Piece = p, SquareColor = (i % 8 + i / 8) % 2 }).Where(p => p.Piece != null && p.Piece.Owner == Player.White).ToArray();
            var blackPieces = pieces.Select((p, i) => new { Piece = p, SquareColor = (i % 8 + i / 8) % 2 }).Where(p => p.Piece != null && p.Piece.Owner == Player.Black).ToArray();

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

            if (whitePieces.Length == 2 && blackPieces.Length == 2) // King and bishop vs king and bishop
            {
                var whiteBishop = whitePieces.First(p => p.GetType().Name == typeof(Bishop).Name);
                var blackBishop = blackPieces.First(p => p.GetType().Name == typeof(Bishop).Name);
                return whiteBishop != null && blackBishop != null &&
                       whiteBishop.SquareColor == blackBishop.SquareColor;
            }
            return false;
        }

        public static List<Move> GetValidMoves(GameBoard board)
        {
            var player = board.WhoseTurn();
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

            var playerOwnedSquares = squares.Where(sq => board[sq] != null &&
                                                         board[sq].Owner == player).ToArray();
            var nonPlayerOwnedSquares = squares.Where(sq => board[sq] == null ||
                                                            board[sq].Owner != player).ToArray();

            foreach (var playerOwnedSquare in playerOwnedSquares)
            {
                validMoves.AddRange(from nonPlayerOwnedSquare in nonPlayerOwnedSquares
                    select new Move(playerOwnedSquare, nonPlayerOwnedSquare, player) into move
                    where GameBoard.IsValidMove(move, board)
                    select move);
            }

            return validMoves;
        }

        public static List<Move> GetValidMovesOfSourceSquare(Square source, GameBoard board)
        {
            var validMoves = new List<Move>();
            var piece = board[source];
            if (piece == null)
            {
                return validMoves;
            }

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

            var player = piece.Owner;
            var nonPlayerOwnedSquares = squares.Where(sq => board[sq] == null ||
                                                            board[sq].Owner != player).ToArray();


            validMoves.AddRange(from nonPlayerOwnedSquare in nonPlayerOwnedSquares
                                select new Move(source, nonPlayerOwnedSquare, player, PawnPromotion.Queen) into move
                                where GameBoard.IsValidMove(move, board)
                                select move);


            return validMoves;
        }

        internal static bool IsPlayerInCheck(Player player, GameBoard board)
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

            var opponentOwnedSquares = squares.Where(sq => board[sq] != null &&
                                                           board[sq].Owner != player);
            var playerKingSquare = squares.First(sq => new King(player).Equals(board[sq]));

            return (from opponentOwnedSquare in opponentOwnedSquares
                    let piece = board[opponentOwnedSquare]
                    let move = new Move(opponentOwnedSquare, playerKingSquare, RevertPlayer(player))
                    where piece.IsValidGameMove(move, board)
                    select piece).Any();
        }

        internal static bool PlayerWillBeInCheck(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            var boardClone = GameBoard.Clone(board); // Make the move on this board to keep original board as is.
            Piece piece = boardClone[move.Source];
            boardClone.Board[(int)move.Source.Rank, (int)move.Source.File] = null;
            boardClone.Board[(int)move.Destination.Rank, (int)move.Destination.File] = piece;

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
