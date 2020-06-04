using System;
using System.Globalization;

namespace ChessSharp.SquareData
{
    internal class Parser
    {
        private static File ParseFile(char file)
        {
            // Culture doesn't really matter here, but to silence CA1304
            file = char.ToUpper(file, CultureInfo.InvariantCulture);
            if (file < 'A' || file > 'H')
            {
                throw new ArgumentOutOfRangeException(nameof(file));
            }

            return (File)(file - 'A');
        }

        private static Rank ParseRank(char rank)
        {
            if (rank < '1' || rank > '8')
            {
                throw new ArgumentOutOfRangeException(nameof(rank));
            }
            return (Rank)(rank - '1');
        }

        public static Square Parse(string square)
        {
            if (square == null)
            {
                throw new ArgumentNullException(nameof(square));
            }
            if (square.Length != 2)
            {
                throw new ArgumentException("Argument length must be 2", nameof(square));
            }

            File file = ParseFile(square[0]);
            Rank rank = ParseRank(square[1]);

            return new Square(file, rank);
        }
    }

    /// <summary>Represents a chess square.</summary>
    public class Square
    {

        /// <summary>
        /// Initializes a new instance of the <c>Square</c>class.
        /// </summary>
        /// <param name="file">A <see cref="SquareData.File"/> enum representing the file of the square.</param>
        /// <param name="rank">A <see cref="SquareData.Rank"/> enum representing the rank of the square.</param>
        public Square(File file, Rank rank)
        {
            File = file;
            Rank = rank;
        }

#pragma warning disable CA2225 // Operator overloads have named alternates - Investigate this later.
        public static implicit operator Square(string s) => Parser.Parse(s);
#pragma warning restore CA2225 // Operator overloads have named alternates - Investigate this later.

        /// <summary>Gets the <see cref="SquareData.File"/> of the square.</summary>
        public File File { get; }

        /// <summary>Gets the <see cref="SquareData.Rank"/> of the square.</summary>
        public Rank Rank { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var square = (Square)obj;
            return square.File == File && square.Rank == Rank;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(File, Rank);
        }

        /// <summary>Converts the given square to a string.</summary>
        /// <returns>Returns string representation of the square. For example, it returns "G2" for a square with G file and second rank.</returns>
        public override string ToString()
        {
            return File.ToString() + ((int)Rank + 1);
        }

    }
}