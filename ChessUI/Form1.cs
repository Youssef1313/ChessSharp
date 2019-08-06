using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChessLibrary;
using ChessLibrary.SquareData;

namespace ChessUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Array.ForEach(Controls.OfType<Label>().Where(m => System.Text.RegularExpressions.Regex.IsMatch(m.Name, "lbl_[A-H][1-8]")).ToArray(), lbl => lbl.BackgroundImageLayout = ImageLayout.Zoom);
        }

        private GameBoard _gameBoard = new GameBoard();
        private void Form1_Load(object sender, EventArgs e)
        {
            DrawBoard();
        }

        private void DrawBoard()
        {
            
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var file = (File)i;
                    var rank = (Rank)j;
                    Label lbl = this.Controls.OfType<Label>()
                        .First(m => m.Name == "lbl_" + file.ToString() + ((int) rank + 1));
                    Piece piece = _gameBoard[file, rank];
                    if (piece == null)
                    {
                        lbl.BackgroundImage = null;
                        continue;
                    }
                    lbl.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject($"{piece.Owner}{piece.GetType().Name}");
                    
                }
                
            }

            lblWhoseTurn.Text = _gameBoard.WhoseTurn().ToString();
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var source = new Square(textBox1.Text[0], (byte)(textBox1.Text[1] - '0'));
                var destination = new Square(textBox2.Text[0], (byte)(textBox2.Text[1] - '0'));
                var move = new Move(source, destination, _gameBoard.WhoseTurn());
                if (!_gameBoard.IsValidMove(move))
                {
                    MessageBox.Show("Invalid Move!", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                _gameBoard.MakeMove(move);
                DrawBoard();
                Player lastPlayer = ChessUtilities.RevertPlayer(_gameBoard.WhoseTurn());
                GameState state = ChessUtilities.GetGameState(_gameBoard.Board, lastPlayer);
                if (state != GameState.NotCompleted)
                {
                    MessageBox.Show(state.ToString());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Error\n{exception.Message}", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
    }
}