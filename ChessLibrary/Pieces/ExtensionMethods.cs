namespace ChessLibrary.Pieces
{
    public static class ExtensionMethods
    {
        public static bool Contains(this Pawn.PawnMoveType moveType1, Pawn.PawnMoveType moveType2) => (moveType1 & moveType2) == moveType2;
    }


}