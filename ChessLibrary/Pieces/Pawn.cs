using System;
using ChessLibrary.PositionData;

namespace ChessLibrary.Pieces
{
    public class Pawn : Piece
    {
        public override Player Owner { get; set; }

        protected override bool IsValidPieceMove(Move move)
        {
            // Check normal one step pawn move.
            if ((move.Player == Player.White && move.GetDeltaY() == 1 && move.GetDeltaX() == 0) ||
                move.Player == Player.Black && move.GetDeltaY() == -1 && move.GetDeltaX() == 0)
            {
                return true;
            }

            // Check two step move from starting position.
            if ((move.Player == Player.White && move.GetDeltaY() == 2 && move.GetDeltaX() == 0 && move.Source.Rank == Rank.Second) ||
                (move.Player == Player.Black && move.GetDeltaY() == -2 && move.GetDeltaX() == 0 && move.Source.Rank == Rank.Seventh))
            {
                return true;
            }

            // Check en-passant.
            return (move.Player == Player.White && move.GetDeltaY() == 1 && move.GetAbsDeltaX() == 1 && move.Source.Rank == Rank.Fifth) ||
                   (move.Player == Player.Black && move.GetDeltaY() == -1 && move.GetAbsDeltaX() == 1 && move.Source.Rank == Rank.Forth);
        }

        public override bool IsValidGameMove(Move move, GameBoard board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (!IsValidPieceMove(move))
            {
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
