using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess_game_2
{
    internal class Drawing
    {
        public static int SquareSize = 120;
        public static int BoardSize = 8;

        Color DarkSquare = Color.DarkGreen;
        Color LightSquare = Color.LightSteelBlue;

        public void DrawBoard(Graphics g)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Color squareColor = ((row + col) % 2 == 0) ? LightSquare : DarkSquare;
                    using (SolidBrush brush = new SolidBrush(squareColor))
                    {
                        g.FillRectangle(brush, col * SquareSize, row * SquareSize, SquareSize, SquareSize);
                    }
                }
            }
        }

        public void DrawCoordinates(Graphics g)
        {
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            for (int i = 0; i < BoardSize; i++)
            {
                string colLabel = ((char)('A' + i)).ToString();
                g.DrawString(colLabel, font, brush, i * SquareSize + SquareSize / 2 - 10, BoardSize * SquareSize);
                string rowLabel = (BoardSize - i).ToString();
                g.DrawString(rowLabel, font, brush, BoardSize * SquareSize, i * SquareSize + SquareSize / 2 - 10);
            }
        }

        public void DrawSelection(Graphics g, Pieces pieces)
        {
            if (!pieces.IsSelected) return;

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(120, Color.Yellow)))
            {
                g.FillRectangle(
                    brush,
                    pieces.SelectedCol * SquareSize,
                    pieces.SelectedRow * SquareSize,
                    SquareSize,
                    SquareSize
                );
            }
        }

        public void DrawTurnIndicator(Graphics g, string currentTurn)
        {
            string text = currentTurn == "white" ? "White's turn" : "Black's turn";
            Color color = currentTurn == "white" ? Color.White : Color.Black;

            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(180, Color.Gray)))
            using (SolidBrush fg = new SolidBrush(color))
            {
                int y = BoardSize * SquareSize + 30;
                g.FillRectangle(bg, 10, y - 4, 200, 30);
                g.DrawString(text, font, fg, 15, y);
            }
        }

        public void DrawPieces(Graphics g, string[,] positions)
        {
            Dictionary<string, string> symbols = new Dictionary<string, string>()
            {
                { "K", "♔" }, { "Q", "♕" }, { "R", "♖" },
                { "B", "♗" }, { "N", "♘" }, { "P", "♙" },
                { "k", "♚" }, { "q", "♛" }, { "r", "♜" },
                { "b", "♝" }, { "n", "♞" }, { "p", "♟" }
            };

            Font font = new Font("Segoe UI Symbol", 48);

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    string piece = positions[row, col];
                    if (!string.IsNullOrEmpty(piece))
                    {
                        string symbol = symbols[piece];
                        Brush brush = char.IsUpper(piece[0]) ? Brushes.White : Brushes.Black;
                        float x = col * SquareSize + 25;
                        float y = row * SquareSize + 20;
                        g.DrawString(symbol, font, brush, x, y);
                    }
                }
            }
        }
    }
}