using System;
using ChessLibrary.SquareData;

namespace ChessLibrary.Pieces
{
    public partial class Pawn : Piece
    {
        public Pawn(Player player) : base(player) { }


        private static PawnMoveType GetPawnMoveType(Move move, GameBoard board)
        {
            PawnMoveType result = PawnMoveType.Invalid;
            sbyte deltaY = move.GetDeltaY();
            byte absDeltaX = move.GetAbsDeltaX();

            // Check promotion move.
            if ((move.Player == Player.White && move.Destination.Rank == Rank.Eighth) ||
                (move.Player == Player.Black && move.Destination.Rank == Rank.First))
            {
                result |= PawnMoveType.Promotion;
            }

            // Check normal one step pawn move.
            if ((move.Player == Player.White && deltaY == 1 && absDeltaX == 0) ||
                move.Player == Player.Black && deltaY == -1 && absDeltaX == 0)
            {
                result |= PawnMoveType.OneStep;
            }

            // Check two step move from starting position.
            if ((move.Player == Player.White && deltaY == 2 && absDeltaX == 0 && move.Source.Rank == Rank.Second) ||
                (move.Player == Player.Black && deltaY == -2 && absDeltaX == 0 && move.Source.Rank == Rank.Seventh))
            {
                result |= PawnMoveType.TwoSteps;
            }
            
            // Check capture (Enpassant is special case from capture).
            if ((move.Player == Player.White && deltaY == 1 && absDeltaX == 1) ||
                (move.Player == Player.Black && deltaY == -1 && absDeltaX == 1))
            {
                result |= PawnMoveType.Capture;
            }

            return result;
        }

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

            var moveType = GetPawnMoveType(move, board);
            if (moveType == PawnMoveType.Invalid)
            {
                return false;
            }

            if (moveType.Contains(PawnMoveType.OneStep))
            {
                return board[move.Destination] == null;
            }

            if (moveType.Contains(PawnMoveType.TwoSteps))
            {
                return !ChessUtilities.IsTherePieceInBetween(move, board.Board) && board[move.Destination] == null;
            }

            if (moveType.Contains(PawnMoveType.Capture))
            {
                // Check regular capture.
                if (board[move.Destination] != null)
                {
                    return true;
                }

                // Check enpassant.
                // TODO: I should check if the player will be in check after enpassant HERE IN Pawn.cs, because it won't work correctly in GameBoard.IsValidMove
                return false;
            }

            if (moveType.Contains(PawnMoveType.Promotion))
            {
                // TODO: Promotion
                return false;
            }


            throw new Exception("Unexpected PawnMoveType.");


            
        }
    }
}