using System;

namespace ChessSharp.Pieces
{
    [Flags]
    internal enum PawnMoveType
    {
        Invalid = 0,
        OneStep = 1,
        TwoSteps = 2,
        Capture = 4,
        Promotion = 8
    }
}