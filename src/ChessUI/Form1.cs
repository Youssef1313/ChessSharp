using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using Microsoft.VisualBasic;

namespace ChessUI
{
    public partial class Form1 : Form
    {
        private readonly Label[] _squareLabels;
        private readonly Dictionary<string, Point> _whiteLocations;
        private readonly Dictionary<string, Point> _blackLocations;
        private Square _selectedSourceSquare;
        private ChessGame _gameBoard = new ChessGame();


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
            if (checkBox1.Checked) return;
            var locationsDictionary = player == Player.White ? _whiteLocations : _blackLocations;
            Array.ForEach(_squareLabels, lbl => lbl.Location = locationsDictionary[lbl.Name]);
        }

        private Player? GetPlayerInCheck()
        {
            if (_gameBoard.GameState == GameState.BlackInCheck || _gameBoard.GameState == GameState.WhiteWinner)
            {
                return Player.Black;
            }
            if (_gameBoard.GameState == GameState.WhiteInCheck || _gameBoard.GameState == GameState.BlackWinner)
            {
                return Player.White;
            }
            return null;
        }

        private void SquaresLabels_Click(object sender, EventArgs e)
        {
            Label selectedLabel = (Label)sender;
            if (selectedLabel.BackColor != Color.DarkCyan)
            {
                // Re-draw to remove previously colored labels.
                DrawBoard(GetPlayerInCheck());

                if (selectedLabel.Tag.ToString() != _gameBoard.WhoseTurn.ToString()) return;
                _selectedSourceSquare = selectedLabel.Name.Substring("lbl_".Length);
                var validDestinations = ChessUtilities.GetValidMovesOfSourceSquare(_selectedSourceSquare, _gameBoard).Select(m => m.Destination).ToArray();
                if (validDestinations.Length == 0) return;
                selectedLabel.BackColor = Color.Cyan;
                Array.ForEach(validDestinations, square =>
                    {
                        _squareLabels.First(lbl => lbl.Name == $"lbl_{square}").BackColor = Color.DarkCyan;
                    });
            }
            else
            {
                MakeMove(_selectedSourceSquare.ToString(), selectedLabel.Name.Substring("lbl_".Length));
            }
        }


        private void DrawBoard(Player? playerInCheck = null)
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var file = (File)i;
                    var rank = (Rank)j;
                    Label lbl = _squareLabels.First(m => m.Name == "lbl_" + file.ToString() + ((int)rank + 1));
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

            if (playerInCheck == null) return;

            // Division => Rank             Modulus => File

            Square checkedKingSquare = _gameBoard.Board.Cast<Piece>()
                .Select((p, i) => new { Piece = p, Square = new Square((File)(i % 8), (Rank)(i / 8)) })
                .First(m => m.Piece is King && m.Piece.Owner == playerInCheck).Square;
            _squareLabels.First(lbl => lbl.Name == "lbl_" + checkedKingSquare).BackColor = Color.Red;


        }

        private void MakeMove(string source, string destination)
        {
            try
            {
                Square squareSource = source;
                Square squareDestination = destination;
                Player player = _gameBoard.WhoseTurn;
                PawnPromotion? pawnPromotion = null;
                if (_gameBoard[squareSource] is Pawn)
                {
                    if ((player == Player.White && squareDestination.Rank == Rank.Eighth) ||
                        (player == Player.Black && squareDestination.Rank == Rank.First))
                    {
                        //var promotion = Interaction.InputBox("Promote to what ?", "Promotion").ToLower();
                        // Interaction.InputBox isn't supported in .NET Core currently.
                        // Consider using it and remove InputBox if it became supported in future release.
                        string promotion;
                        using (var inputBox = new InputBox())
                        {
                            inputBox.ShowDialog();
                            promotion = inputBox.UserInput;
                        }
                        pawnPromotion = (PawnPromotion) Enum.Parse(typeof(PawnPromotion), promotion, true);
                    }
                }

                var move = new Move(squareSource, squareDestination, player, pawnPromotion);
                if (!_gameBoard.IsValidMove(move))
                {
                    MessageBox.Show("Invalid Move!", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                _gameBoard.MakeMove(move, isMoveValidated: true);
                
                DrawBoard(GetPlayerInCheck());

                if (_gameBoard.GameState == GameState.Draw || _gameBoard.GameState == GameState.Stalemate ||
                    _gameBoard.GameState == GameState.BlackWinner || _gameBoard.GameState == GameState.WhiteWinner)
                {
                    MessageBox.Show(_gameBoard.GameState.ToString());
                    return;
                }

                Player whoseTurn = _gameBoard.WhoseTurn;
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