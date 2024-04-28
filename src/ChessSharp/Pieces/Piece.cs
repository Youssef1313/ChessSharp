using System;
using System.Diagnostics.CodeAnalysis;

namespace ChessSharp.Pieces;

/// <summary>Represents the base class of the pieces.</summary>
public abstract class Piece
{

    /// <summary>Gets the owner <see cref="Player"/> of the piece.</summary>
    public Player Owner { get; }

    internal abstract bool IsValidGameMove(Move move, ChessGame board);


    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Piece p && p.GetType() == GetType() && Owner == p.Owner;

    public override int GetHashCode() => HashCode.Combine(GetType(), Owner);

    protected Piece(Player player) => Owner = player;

}
