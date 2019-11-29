namespace ChessSharp
{
    /// <summary>Specifies the state of the game.</summary>
    public enum GameState
    {
        /// <summary>The game is still in progress.</summary>
        NotCompleted,
        /// <summary>The game is over and white has won.</summary>
        WhiteWinner,
        /// <summary>The game is over and black has won.</summary>
        BlackWinner,
        /// <summary>The game is over and ended in a draw.</summary>
        Draw,
        /// <summary>The game is over and ended in a stalemate.</summary>
        Stalemate,
        /// <summary>The game is not over and white is in check.</summary>
        WhiteInCheck,
        /// <summary>The game is not over and black is in check.</summary>
        BlackInCheck
    }
}