using System;
using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace ChessSharpUno;

public sealed partial class BoardSquare : Grid
{
    private readonly Image _image;
    private readonly ChessBoardGrid _boardGrid;
    private readonly File _file;
    private readonly Rank _rank;

    public BoardSquare(File file, Rank rank, ChessBoardGrid boardGrid)
    {
        Width = 60;
        Height = 60;
        _file = file;
        _rank = rank;
        _image = new Image();
        _boardGrid = boardGrid;
        Children.Add(_image);
        OnStatusChanged(BoardSquareStatus.Unselected);
        PointerReleased += OnSquareClick;
    }

    private void OnSquareClick(object sender, PointerRoutedEventArgs e)
        => _boardGrid.ClickSquare(_file, _rank);

    public Piece? Piece
    {
        get => (Piece?)GetValue(PieceProperty);
        set => SetValue(PieceProperty, value);
    }

    public static DependencyProperty PieceProperty { get; } = DependencyProperty.Register(
        nameof(Piece),
        typeof(Piece),
        typeof(BoardSquare), new PropertyMetadata(null, propertyChangedCallback: (sender, args) => ((BoardSquare)sender).OnPieceChanged(args)));



    public BoardSquareStatus Status
    {
        get => (BoardSquareStatus)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public static DependencyProperty StatusProperty { get; } = DependencyProperty.Register(
        nameof(Status),
        typeof(BoardSquareStatus),
        typeof(BoardSquare),
        new PropertyMetadata(
            BoardSquareStatus.Unselected,
            propertyChangedCallback: (sender, args) => ((BoardSquare)sender).OnStatusChanged((BoardSquareStatus)args.NewValue)));

    private void OnStatusChanged(BoardSquareStatus status)
    {
        switch (status)
        {
            case BoardSquareStatus.Unselected:
                var i = (int)_file;
                var j = (int)_rank;
                var backgroundColor = ((i + j) % 2 == 0) ? Color.FromArgb(255, 181, 136, 99) : Color.FromArgb(255, 240, 217, 181);
                Background = new SolidColorBrush(backgroundColor);
                break;

            case BoardSquareStatus.Selected:
                Background = new SolidColorBrush(Colors.Cyan);
                break;

            case BoardSquareStatus.ValidMove:
                Background = new SolidColorBrush(Colors.DarkCyan);
                break;

            case BoardSquareStatus.InCheck:
                Background = new SolidColorBrush(Colors.Red);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(status));
        }
    }

    private void OnPieceChanged(DependencyPropertyChangedEventArgs args)
    {
        var piece = (Piece)args.NewValue;
        if (piece is not null)
        {
            var color = piece.Owner.ToString();
            var pieceName = piece.GetType().Name;
            _image.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Images/{color}{pieceName}.png"));
        }
        else
        {
            _image.Source = null;
        }
    }
}
