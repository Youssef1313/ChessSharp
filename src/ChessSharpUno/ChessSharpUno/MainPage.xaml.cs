using System.Collections.Generic;
using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ChessSharpUno;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        whoseTurnTextBlock.Text = chessBoard.WhoseTurn;
    }

    private void chessBoard_MoveMade()
    {
        whoseTurnTextBlock.Text = chessBoard.WhoseTurn;
    }
}
