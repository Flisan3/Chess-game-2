using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess_game_2
{
    public partial class Form1 : Form
    {
        Pieces pieces = new Pieces();
        Drawing boardDrawer = new Drawing();

        public Form1()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;
            this.MouseDown += Form1_MouseDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            boardDrawer.DrawBoard(e.Graphics);
            boardDrawer.DrawCoordinates(e.Graphics);
            boardDrawer.DrawSelection(e.Graphics, pieces);
            boardDrawer.DrawPieces(e.Graphics, pieces.positions);
            boardDrawer.DrawTurnIndicator(e.Graphics, pieces.CurrentTurn);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int col = e.X / 120;
            int row = e.Y / 120;

            pieces.HandleClick(row, col);
            Invalidate();
        }
    }
}
