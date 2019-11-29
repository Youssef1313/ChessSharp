using System;
using ChessSharp.SquareData;

namespace ChessSharp.Pieces
{
    /// <summary>Represents a king <see cref="Piece"/>.</summary>
    public class King : Piece
    {
        internal King(Player player) : base(player) { }
        
        internal override bool IsValidGameMove(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            byte absDeltaX = move.GetAbsDeltaX();
            byte absDeltaY = move.GetAbsDeltaY();

            // Regular king move.
            if (move.GetAbsDeltaX() <= 1 && move.GetAbsDeltaY() <= 1)
            {
                return true;
            }
            // Not castle move.
            if (absDeltaX != 2 || absDeltaY != 0 || move.Source.File != File.E ||
                (move.Player == Player.White && move.Source.Rank != Rank.First) ||
                (move.Player == Player.Black && move.Source.Rank != Rank.Eighth) ||
                (board.GameState == GameState.BlackInCheck || board.GameState == GameState.WhiteInCheck))
            {
                return false;
            }

            // White king-side castle move.
            if (move.Player == Player.White && move.Destination.File == File.G && board.CanWhiteCastleKingSide &&
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.H, Rank.First), board.Board) &&
                new Rook(Player.White).Equals(board[File.H, Rank.First]))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.F, Rank.First), move.Player), board);
            }

            // Black king-side castle move.
            if (move.Player == Player.Black && move.Destination.File == File.G && board.CanBlackCastleKingSide &&
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.H, Rank.Eighth), board.Board) &&
                new Rook(Player.Black).Equals(board[File.H, Rank.Eighth]))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.F, Rank.Eighth), move.Player), board);
            }

            // White queen-side castle move.
            if (move.Player == Player.White && move.Destination.File == File.C && board.CanWhiteCastleQueenSide &&
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.A, Rank.First), board.Board) &&
                new Rook(Player.White).Equals(board[File.A, Rank.First]))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.D, Rank.First), move.Player), board);
            }

            // Black queen-side castle move.
            if (move.Player == Player.Black && move.Destination.File == File.C && board.CanBlackCastleQueenSide &&
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.A, Rank.Eighth), board.Board) &&
                new Rook(Player.Black).Equals(board[File.A, Rank.Eighth]))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.D, Rank.Eighth), move.Player), board);
            }

            return false;

        }
    }
}
