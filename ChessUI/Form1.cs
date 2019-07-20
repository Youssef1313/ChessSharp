using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChessLibrary;
using ChessLibrary.PositionData;

namespace ChessUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                        lbl.Text = "";
                        continue;
                    }
                    lbl.Text = piece.GetType().Name;
                    lbl.ForeColor = (piece.Owner == Player.Black) ? Color.Black : Color.White;
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
                var source = new Position(textBox1.Text[0], (byte)(textBox1.Text[1] - '0'));
                var destination = new Position(textBox2.Text[0], (byte)(textBox2.Text[1] - '0'));
                var move = new Move(source, destination, _gameBoard.WhoseTurn());
                if (!_gameBoard.IsValidMove(move))
                {
                    MessageBox.Show("Invalid Move!", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                _gameBoard.MakeMove(move);
                DrawBoard();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Error\n{exception.Message}", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
