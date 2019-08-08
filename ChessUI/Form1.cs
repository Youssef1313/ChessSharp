using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ChessLibrary;
using ChessLibrary.Pieces;
using ChessLibrary.SquareData;

namespace ChessUI
{
    public partial class Form1 : Form
    {
        private Square _selectedSourceSquare;
        private readonly Label[] _squareLabels;
        private readonly Dictionary<string, Point> _whiteLocations;
        private readonly Dictionary<string, Point> _blackLocations;
        private GameBoard _gameBoard = new GameBoard();


        private static string InvertSquare(string sq)
        {
            // sq is like lbl_A7 for example.
            // file char at index 4,
            // rank char at index 5.
            var f = (char)('A' + 'H' - sq[4]);
            var r = '9' - sq[5];
            return "lbl_" + f + r;
        }

        public Form1()
        {
            InitializeComponent();
            _squareLabels = Controls.OfType<Label>()
                                 .Where(m => Regex.IsMatch(m.Name, "lbl_[A-H][1-8]")).ToArray();

            Array.ForEach(_squareLabels, lbl =>
            {
                lbl.BackgroundImageLayout = ImageLayout.Zoom;
                lbl.Click += SquaresLabels_Click;
            });




            _whiteLocations = _squareLabels.ToDictionary(lbl => lbl.Name,
                                                        lbl => lbl.Location);

            _blackLocations = _squareLabels.ToDictionary(lbl => InvertSquare(lbl.Name),
                                                        lbl => lbl.Location);

            DrawBoard();
        }

        private void FlipUi(Player player)
        {
            var locationsDictionary = player == Player.White ? _whiteLocations : _blackLocations;
            Array.ForEach(_squareLabels, lbl => lbl.Location = locationsDictionary[lbl.Name]);
        }


        private void SquaresLabels_Click(object sender, EventArgs e)
        {
            Label selectedLabel = (Label)sender;
            if (selectedLabel.BackColor != Color.DarkCyan)
            {
                DrawBoard(); // Remove previously colored labels.
                if (selectedLabel.Tag.ToString() != _gameBoard.WhoseTurn().ToString()) return;
                _selectedSourceSquare = Square.Parse(selectedLabel.Name.Substring("lbl_".Length));
                var validDestinations = ChessUtilities.GetValidMovesOfSourceSquare(_selectedSourceSquare, _gameBoard).Select(m => m.Destination).ToArray();
                if (validDestinations.Length == 0) return;
                selectedLabel.BackColor = Color.Cyan;
                Array.ForEach(validDestinations, square =>
                    {
                        _squareLabels.First(lbl => lbl.Name == "lbl_" + square.ToString()).BackColor = Color.DarkCyan;
                    });
            }
            else
            {
                MakeMove(_selectedSourceSquare.ToString(), selectedLabel.Name.Substring("lbl_".Length));
            }
        }

 
        private void DrawBoard()
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var file = (File)i;
                    var rank = (Rank)j;
                    Label lbl = _squareLabels.First(m => m.Name == "lbl_" + file.ToString() + ((int) rank + 1));
                    Piece piece = _gameBoard[file, rank];
                    lbl.BackColor = ((i + j) % 2 == 0) ? Color.FromArgb(181, 136, 99) : Color.FromArgb(240, 217, 181);
                    if (piece == null)
                    {
                        lbl.BackgroundImage = null;
                        lbl.Tag = "empty";
                        continue;
                    }
                    lbl.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject($"{piece.Owner}{piece.GetType().Name}");
                    lbl.Tag = piece.Owner.ToString();
                }
                
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeMove(textBox1.Text, textBox2.Text);
            textBox1.Clear();
            textBox2.Clear();
        }

        private void MakeMove(string source, string destination)
        {
            try
            {
                var squareSource = Square.Parse(source);
                var squareDestination = Square.Parse(destination);
                Player player = _gameBoard.WhoseTurn();
                var move = new Move(squareSource, squareDestination, player);
                if (!_gameBoard.IsValidMove(move))
                {
                    MessageBox.Show("Invalid Move!", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                _gameBoard.MakeMove(move);
                DrawBoard();
                
                GameState state = ChessUtilities.GetGameState(_gameBoard);
                if (state == GameState.Draw || state == GameState.Stalemate ||
                    state == GameState.BlackWinner || state == GameState.WhiteWinner)
                {
                    MessageBox.Show(state.ToString());
                    return;
                }

                if (state == GameState.BlackInCheck)
                {
                    // Division => Rank             Modulus => File

                    Square blackKingSquare = _gameBoard.Board.Cast<Piece>()
                        .Select((p, i) => new { Piece = p, Square = new Square((File)(i / 8), (Rank)(i / 8)) })
                        .First(m => new King(Player.Black).Equals(m.Piece)).Square;
                    _squareLabels.First(lbl => lbl.Name == "lbl_" + blackKingSquare).BackColor = Color.Red;
                }

                if (state == GameState.WhiteInCheck)
                {
                    // Division => Rank             Modulus => File

                    Square whiteKingSquare = _gameBoard.Board.Cast<Piece>()
                        .Select((p, i) => new { Piece = p, Square = new Square((File)(i % 8), (Rank)(i / 8)) })
                        .First(m => new King(Player.White).Equals(m.Piece)).Square;
                    _squareLabels.First(lbl => lbl.Name == "lbl_" + whiteKingSquare).BackColor = Color.Red;
                }

                Player whoseTurn = _gameBoard.WhoseTurn();
                lblWhoseTurn.Text = whoseTurn.ToString();
                FlipUi(whoseTurn);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Error\n{exception.Message}", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}