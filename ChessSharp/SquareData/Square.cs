using System;

namespace ChessSharp.SquareData
{
    public class Square
    {
        public static implicit operator Square(string s) => Square.Parse(s);

        public File File { get; }
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

        public override string ToString()
        {
            return File.ToString() + ((int) Rank + 1);
        }

        public Square(File file, Rank rank)
        {
            File = file;
            Rank = rank;
        }

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
            File tempFile;
            Enum.TryParse(file.ToString(), out tempFile);
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