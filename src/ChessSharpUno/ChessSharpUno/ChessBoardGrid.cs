using System;
using System.Collections.Generic;
using ChessSharp;
using ChessSharp.SquareData;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ChessSharpUno;

public sealed partial class ChessBoardGrid : Grid
{
    private readonly ChessGame _game = new();
    private Dictionary<Square, BoardSquare> _boardSquareMap = new();

    private Square? _selectedSquare;
    private List<Square> _validMoves = new();

    public string WhoseTurn { get; private set; }

    public ChessBoardGrid()
    {
        for (int i = 0; i < 8; i++)
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        }

        for (File file = File.A; file <= File.H; file++)
        {
            for (Rank rank = Rank.First; rank <= Rank.Eighth; rank++)
            {
                var boardSquare = new BoardSquare(file, rank, this);
                boardSquare.Piece = _game[file, rank];
                Grid.SetRow(boardSquare, 7 - (int)rank);
                Grid.SetColumn(boardSquare, (int)file);
                Children.Add(boardSquare);
                _boardSquareMap[new Square(file, rank)] = boardSquare;
            }
        }

        WhoseTurn = _game.WhoseTurn.ToString();
    }

    private void UpdateBoard()
    {
        for (File file = File.A; file <= File.H; file++)
        {
            for (Rank rank = Rank.First; rank <= Rank.Eighth; rank++)
            {
                _boardSquareMap[new Square(file, rank)].Piece = _game[file, rank];
            }
        }
    }

    public event Action MoveMade;

    public void ClickSquare(File file, Rank rank)
    {
        if (_selectedSquare.HasValue)
        {
            _boardSquareMap[_selectedSquare.Value].Status = BoardSquareStatus.Unselected;
        }

        foreach (var oldValidMove in _validMoves)
        {
            _boardSquareMap[oldValidMove].Status = BoardSquareStatus.Unselected;
        }

        var clickedSquare = new Square(file, rank);
        if (_selectedSquare.HasValue && _validMoves.Contains(clickedSquare))
        {
            _game.MakeMove(new Move(_selectedSquare.Value, clickedSquare, _game.WhoseTurn), isMoveValidated: true);
            
            try
            {
                WhoseTurn = _game.WhoseTurn.ToString();
                MoveMade?.Invoke();
            }
            catch { }

            _selectedSquare = null;
            _validMoves.Clear();

            UpdateBoard();
        }
        else
        {
            _validMoves.Clear();
            var moves = ChessUtilities.GetValidMovesOfSourceSquare(clickedSquare, _game);
            if (moves.Count > 0)
            {
                _selectedSquare = clickedSquare;
                _boardSquareMap[clickedSquare].Status = BoardSquareStatus.Selected;
                foreach (var move in moves)
                {
                    _validMoves.Add(move.Destination);
                    var possibleDestination = _boardSquareMap[move.Destination];
                    possibleDestination.Status = BoardSquareStatus.ValidMove;
                }
            }
        }
    }
}
