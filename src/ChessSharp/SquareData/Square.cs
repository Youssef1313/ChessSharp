using System;

namespace ChessSharp.SquareData
{
    
    /// <summary>Represents a chess square.</summary>
    public class Square
    {
        public static implicit operator Square(string s) => Square.Parse(s);

        /// <summary>Gets the <see cref="SquareData.File"/> of the square.</summary>
        public File File { get; }
        /// <summary>Gets the <see cref="SquareData.Rank"/> of the square.</summary>
        public Rank Rank { get; }

        public override bool Equals(object obj)
        {

            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var square = (Square) obj;
            return square.File == this.File && square.Rank == this.Rank;
        }

        public override int GetHashCode()
        {
            // Jon skeet's implementation:
            // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode

            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + File.GetHashCode();
                hash = hash * 23 + Rank.GetHashCode();
                return hash;
            }
        }

        /// <summary>Converts the given square to a string.</summary>
        /// <returns>Returns string representation of the square. For example, it returns "G2" for a square with G file and second rank.</returns>
        public override string ToString()
        {
            return File.ToString() + ((int) Rank + 1);
        }

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

        /// <summary>
        /// Initializes a new instance of the Square class.
        /// </summary>
        /// <param name="file">A char representing the file of the square.</param>
        /// <param name="rank">A char representing the rank of the square.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Either <c>file</c> or <c>rank</c> is out of range. <c>file</c> should be a char between 'A' and 'H' (case insensitive). <c>rank</c> should be a char between '1' and '8'.
        /// </exception>
        public Square(char file, byte rank)
        {
            File = ParseFile(file);
            Rank = ParseRank(rank);
        }

        private static File ParseFile(char file)
        {
            file = char.ToUpper(file);
            if (file < 'A' || file > 'H')
            {
                throw new ArgumentOutOfRangeException(nameof(file));
            }
            Enum.TryParse(file.ToString(), out File tempFile);
            return tempFile;
        }

        private static Rank ParseRank(byte rank)
        {
            if (rank < 1 || rank > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(rank));
            }
            return (Rank)(rank - 1);
        }

        private static Rank ParseRank(char rank)
        {
            if (rank < '1' || rank > '8')
            {
                throw new ArgumentOutOfRangeException(nameof(rank));
            }
            return (Rank)(rank - '0' - 1);
        }

        /// <summary>Parses a string representation of the square.</summary>
        /// <param name="square">The string representation of the square.</param>
        /// <returns>Returns a Square object.</returns>
        /// <exception cref="ArgumentNullException">
        ///     The given <c>square</c> string is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     The given <c>square</c> string has length less or greater than two. The <c>square</c> string should have length of two (For example, "G2").
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Either the file or the rank char of the <c>square</c> string is out of range. The file should be between 'A' and 'H' (case insensitive). The <c>rank</c> should be between '1' and '8'.
        /// </exception>
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

            File file = ParseFile(char.ToUpper(square[0]));
            Rank rank = ParseRank(square[1]);
            
            return new Square(file, rank);
        }
    }
}