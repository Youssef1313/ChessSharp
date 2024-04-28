using ChessSharp.Pieces;
using ChessSharp.SquareData;
using NUnit.Framework;

namespace ChessSharp.UnitTests;

[TestFixture]
public class PieceTests
{
    [Test]
    public void PieceEquals_SameOwnerButDifferentPieceType_ReturnsFalse()
    {
        var game = new ChessGame();
        var p1 = game[File.A, Rank.First]; // White rook.
        Assert.True(p1 is Rook && p1.Owner == Player.White);

        var p2 = game[File.B, Rank.First]; // White Knight
        Assert.True(p2 is Knight && p2.Owner == Player.White);

        Assert.False(p1!.Equals(p2));
        Assert.False(p2!.Equals(p1));
    }

    [Test]
    public void PieceEquals_SameReference_ReturnsTrue()
    {
        var game = new ChessGame();
        var p1 = game[File.A, Rank.First]; // White rook.
        Assert.True(p1 is Rook && p1.Owner == Player.White);

        Assert.True(ReferenceEquals(p1, p1));
        Assert.True(p1!.Equals(p1));
    }

    [Test]
    public void PieceEquals_DifferentReferenceButSameOwnerAndType_ReturnsTrue()
    {
        var game = new ChessGame();
        var p1 = game[File.A, Rank.First]; // White rook.
        Assert.True(p1 is Rook && p1.Owner == Player.White);

        game = new ChessGame();
        var p2 = game[File.A, Rank.First]; // White rook.
        Assert.True(p2 is Rook && p2.Owner == Player.White);

        Assert.False(ReferenceEquals(p1, p2));
        Assert.True(p1!.Equals(p2));
    }
}
