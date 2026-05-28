using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    internal class Drawing
    {
 
        // Variables
        public static int SquareSize = 120;
        public static int BoardSize = 8;

        public bool currentTurnIsWhite = true;

        Color DarkSquare = Color.DarkGreen;
        Color LightSquare = Color.LightSteelBlue;

        // Draws the chessboard with alternating colors.
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

        // Draws the coordinate labels around the board.
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

        // Highlights the selected piece's square.
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

        // Displays the current players turn below the board.
        public void DrawTurnIndicator(Graphics g, string currentTurn)
        {
            string text = currentTurn == "white" ? "White's turn" : "Black's turn";
            Color color = currentTurn == "white" ? Color.White : Color.Black;
            currentTurnIsWhite = currentTurn == "white" ? true : false;

            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(180, Color.Gray)))
            using (SolidBrush fg = new SolidBrush(color))
            {
                int y = BoardSize * SquareSize + 30;
                g.FillRectangle(bg, 10, y - 4, 200, 30);
                g.DrawString(text, font, fg, 15, y);
            }
        }

        // Draws the chess pieces on the board using Unicode symbols.
        public void DrawPieces(Graphics g, string[,] positions, bool isCheckmate, bool isStalemate)
        {
            Dictionary<string, string> symbols = new Dictionary<string, string>()
            {
                { "K", "♔" }, { "Q", "♕" }, { "R", "♖" },
                { "B", "♗" }, { "N", "♘" }, { "P", "♙" },
                { "k", "♚" }, { "q", "♛" }, { "r", "♜" },
                { "b", "♝" }, { "n", "♞" }, { "p", "♟" }
            };

            Font font = new Font("Segoe UI Symbol", 48);
            Font specialFont = new Font("Arial", 48, FontStyle.Bold);

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    string piece = positions[row, col];
                    if (!string.IsNullOrEmpty(piece))
                    {
                        string symbol = symbols[piece];
                        Brush brush = char.IsUpper(piece[0]) ? Brushes.White : Brushes.Black;
                        Brush mateBrush = Brushes.Red;
                        float x = col * SquareSize + 25;
                        float y = row * SquareSize + 20;

                        bool isKing = piece == "K" || piece == "k";
                        bool isCheckmatedKing = (piece == "K" && !currentTurnIsWhite) || (piece == "k" && currentTurnIsWhite);

                        if (isCheckmate && isCheckmatedKing)
                        {
                            g.DrawString("#", specialFont, mateBrush, x, y);
                        }
                        else if (isKing && isStalemate)
                        {
                            g.DrawString("½", specialFont, mateBrush, x, y);
                        }
                        else
                        {
                            g.DrawString(symbols[piece], font, brush, x, y);
                        }
                    }
                }
            }
        }

        // Highlights the valid moves for the selected piece.
        public void DrawValidMoves(Graphics g, List<(int row, int col)> validMoves)
        {
            if (validMoves == null) return;

            foreach (var (row, col) in validMoves)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(140, Color.LimeGreen)))
                {
                    g.FillRectangle(brush, col * SquareSize, row * SquareSize, SquareSize, SquareSize);
                }
            }
        }
    }
}