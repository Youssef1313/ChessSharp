using System;

namespace ChessLibrary.PositionData
{
    public class Position
    {
        public File File { get; }
        public Rank Rank { get; }

        public override bool Equals(object obj)
        {

            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var positionObj = (Position) obj;
            return positionObj.File == this.File && positionObj.Rank == this.Rank;
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

        public Position(File file, Rank rank)
        {
            File = file;
            Rank = rank;
        }

        public Position(char file, byte rank)
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
    }
}