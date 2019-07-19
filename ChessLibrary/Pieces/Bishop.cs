using System;
using ChessLibrary.PositionData;

namespace ChessLibrary.Pieces
{
    public class Bishop : Piece
    {
        public override Player Owner { get; set; }

        protected override bool IsValidPieceMove(Move move)
        {
            byte deltaX = move.GetAbsDeltaX();
            if (deltaX == 0)
            {
                return false;
            }
            return deltaX == move.GetAbsDeltaY();
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

            if (board[move.Destination].Owner == move.Player)
            {
                return false; // Can't take your own piece.
            }

            if (ChessUtilities.PlayerWillBeInCheck(move, board))
            {
                return false;
            }

            var xStep = (move.Destination.File > move.Source.File) ? 1 : -1;
            var yStep = (move.Destination.Rank > move.Source.Rank) ? 1 : -1;

            var source = new Position(move.Source.File, move.Source.Rank);
            var destination = new Position(move.Source.File, move.Source.Rank);
            while (source.Rank != destination.Rank)
            {
                source.Rank += yStep;
                source.File += xStep;
                if (board[source] != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}