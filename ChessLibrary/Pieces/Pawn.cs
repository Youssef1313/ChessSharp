using System;
using ChessLibrary.SquareData;

namespace ChessLibrary.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Player player) : base(player) { }

        private enum PawnMoveType
        {
            OneStep,
            TwoSteps,
            Capture,
            Enpassant,
            Invalid
        }

        private static PawnMoveType GetPawnMoveType(Move move)
        {
            sbyte deltaY = move.GetDeltaY();
            byte absDeltaX = move.GetAbsDeltaX();

            // Check normal one step pawn move.
            if ((move.Player == Player.White && deltaY == 1 && absDeltaX == 0) ||
                move.Player == Player.Black && deltaY == -1 && absDeltaX == 0)
            {
                return PawnMoveType.OneStep;
            }

            // Check two step move from starting position.
            if ((move.Player == Player.White && deltaY == 2 && absDeltaX == 0 && move.Source.Rank == Rank.Second) ||
                (move.Player == Player.Black && deltaY == -2 && absDeltaX == 0 && move.Source.Rank == Rank.Seventh))
            {
                return PawnMoveType.TwoSteps;
            }
            // TODO: Maybe a bug ? not sure. In some cases, normal capture may be recognized as enpassant
            // Check en-passant.
            if ((move.Player == Player.White && deltaY == 1 && absDeltaX == 1 && move.Source.Rank == Rank.Fifth) ||
                   (move.Player == Player.Black && deltaY == -1 && absDeltaX == 1 && move.Source.Rank == Rank.Forth))
            {
                return PawnMoveType.Enpassant;
            }


            // Check capture.
            if ((move.Player == Player.White && deltaY == 1 && absDeltaX == 1) ||
                   (move.Player == Player.Black && deltaY == -1 && absDeltaX == 1))
            {
                return PawnMoveType.Capture;
            }

            return PawnMoveType.Invalid;
        }

        internal override bool IsValidGameMove(Move move, Piece[,] board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            var moveType = GetPawnMoveType(move);
            // TODO: Promotion
            switch (moveType)
            {
                case PawnMoveType.Invalid:
                    return false;
                case PawnMoveType.OneStep:
                    return board[(int)move.Destination.Rank, (int)move.Destination.File] == null;
                case PawnMoveType.TwoSteps:
                    return !ChessUtilities.IsTherePieceInBetween(move, board) && board[(int)move.Destination.Rank, (int)move.Destination.File] == null;
                case PawnMoveType.Capture:
                    return board[(int)move.Destination.Rank, (int)move.Destination.File] != null;
                case PawnMoveType.Enpassant:
                    // TODO: En-passant check.
                    return true;
                default:
                    throw new Exception("Unexpected PawnMoveType.");
            }

            
        }
    }
}
