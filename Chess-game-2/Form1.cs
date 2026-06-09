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
        // Main game state and drawing logic
        Pieces pieces = new Pieces();
        Drawing boardDrawer = new Drawing();

        public Form1()
        {
            InitializeComponent();
            // Enable double buffering to reduce flickering
            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            Drawing.RecalculateSquareSize(ClientSize.Height, ClientSize.Width);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Drawing.RecalculateSquareSize(ClientSize.Height, ClientSize.Width);
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Sync turn state before any drawing so IsFlipped is correct
            boardDrawer.CurrentTurnIsWhite = pieces.CurrentTurn == "white";

            boardDrawer.DrawBoard(e.Graphics);
            boardDrawer.DrawCoordinates(e.Graphics);
            boardDrawer.DrawSelection(e.Graphics, pieces);
            boardDrawer.DrawCheck(e.Graphics, pieces.positions, pieces.WhiteKingInCheck, pieces.BlackKingInCheck);
            boardDrawer.DrawValidMoves(e.Graphics, pieces.ValidMoves);
            boardDrawer.DrawPieces(e.Graphics, pieces.positions, pieces.CurrentGameState == GameState.Checkmate, pieces.CurrentGameState == GameState.Stalemate);
            boardDrawer.DrawTurnIndicator(e.Graphics, pieces.CurrentTurn);
        }

        private async void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Calculate the clicked row and column based on mouse coordinates,
            // Then convert from screen coords to logical board coords
            int screenCol = e.X / Drawing.SquareSize;
            int screenRow = e.Y / Drawing.SquareSize;
            int logicalRow = boardDrawer.ToLogicalRow(screenRow);
            int logicalCol = boardDrawer.ToLogicalCol(screenCol);

            //Reset the game if its over
            if (pieces.CurrentGameState == GameState.Checkmate ||
                   pieces.CurrentGameState == GameState.Stalemate)
            {
                pieces = new Pieces();
                Invalidate();
                return;
            }
            //Handle clicking and redraw the board
            pieces.HandleClick(logicalRow, logicalCol);
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            base.OnKeyDown(e);
        }

        private void flipCheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            boardDrawer.FlipBoardEnabled = flipCheckBox.Checked;
            Invalidate();
        }
    }
}