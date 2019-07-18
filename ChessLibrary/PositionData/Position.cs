using System;

namespace ChessLibrary.PositionData
{
    public class Position
    {
        public File File { get; set; }
        public Rank Rank { get; set; }

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