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
