using System;
using ChessLibrary.SquareData;

namespace ChessLibrary.Pieces
{
    public class King : Piece
    {
        public King(Player player) : base(player) { }
        
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
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.H, Rank.First), board.Board))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.F, Rank.First), move.Player), board);
            }

            // Black king-side castle move.
            if (move.Player == Player.Black && move.Destination.File == File.G && board.CanBlackCastleKingSide &&
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.H, Rank.Eighth), board.Board))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.F, Rank.Eighth), move.Player), board);
            }

            // White queen-side castle move.
            if (move.Player == Player.White && move.Destination.File == File.C && board.CanWhiteCastleQueenSide &&
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.A, Rank.First), board.Board))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.D, Rank.First), move.Player), board);
            }

            // Black queen-side castle move.
            if (move.Player == Player.Black && move.Destination.File == File.C && board.CanBlackCastleQueenSide &&
                !ChessUtilities.IsTherePieceInBetween(move.Source, new Square(File.A, Rank.Eighth), board.Board))
            {
                return !ChessUtilities.PlayerWillBeInCheck(
                    new Move(move.Source, new Square(File.D, Rank.Eighth), move.Player), board);
            }

            return false;

        }
    }
}
