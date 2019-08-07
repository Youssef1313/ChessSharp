using ChessLibrary.Pieces;

namespace ChessLibrary
{
    public static class ExtensionMethods
    {
        public static bool Contains(this PawnMoveType moveType1, PawnMoveType moveType2) => (moveType1 & moveType2) == moveType2;
    }


}