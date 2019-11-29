using ChessSharp.Pieces;

namespace ChessSharp
{
    public static class ExtensionMethods
    {
        public static bool Contains(this PawnMoveType moveType1, PawnMoveType moveType2) => (moveType1 & moveType2) == moveType2;
    }


}