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
            //enable double buffering to reduce flickering
            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Draw the board and pieces
            boardDrawer.DrawBoard(e.Graphics);
            boardDrawer.DrawCoordinates(e.Graphics);
            boardDrawer.DrawSelection(e.Graphics, pieces);
            boardDrawer.DrawValidMoves(e.Graphics, pieces.ValidMoves);
            boardDrawer.DrawPieces(e.Graphics,pieces.positions,pieces.CurrentGameState == GameState.Checkmate,pieces.CurrentGameState == GameState.Stalemate);
            boardDrawer.DrawTurnIndicator(e.Graphics, pieces.CurrentTurn);        
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            CheckDetector detector = new CheckDetector();
            if (pieces.CurrentGameState == GameState.Checkmate || pieces.CurrentGameState == GameState.Stalemate)
            {
                // Reset the game if it's over
                pieces = new Pieces();
                Invalidate();
                return;
            }

            // Calculate the clicked row and column based on mouse coordinates
            int col = e.X / 120;
            int row = e.Y / 120;

            pieces.HandleClick(row, col);
            Invalidate();
        }
    }
}
