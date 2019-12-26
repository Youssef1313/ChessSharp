using ChessSharp.Pieces;

namespace ChessSharp
{
    internal static class ExtensionMethods
    {
        public static bool Contain(this PawnMoveType moveType1, PawnMoveType moveType2) => (moveType1 & moveType2) == moveType2;
    }
}
