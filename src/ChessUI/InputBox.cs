using System;
using System.Windows.Forms;

namespace ChessUI;

public partial class InputBox : Form
{
    public string? UserInput { get; private set; }

    public InputBox()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        UserInput = textBox1.Text;
        Close();
    }
}
