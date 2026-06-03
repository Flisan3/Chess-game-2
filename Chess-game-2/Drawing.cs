using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess_game_2
{
    internal class Drawing
    {
        public static GameState CurrentGameState { get; set; }

        // Variables
        public static int BoardSize = 8;
        public static int SquareSize { get; private set; } = 80;

        public bool currentTurnIsWhite = true;

        Color DarkSquare = Color.DarkGreen;
        Color LightSquare = Color.LightSteelBlue;

        public static void RecalculateSquareSize(int availableHeight, int availableWidth)
        {
            int margin = 20;
            int maxByHeight = (availableHeight - margin) / BoardSize;
            int maxByWidth = (availableWidth - margin) / BoardSize;
            SquareSize = Math.Max(40, Math.Min(maxByHeight, maxByWidth));
        }

        public static void RecalculateSquareSizeFromScreen()
        {
            var screen = Screen.PrimaryScreen.WorkingArea;
            RecalculateSquareSize(screen.Height, screen.Width);
        }

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
            float fontSize = Math.Max(8, SquareSize / 10f);
            using (Font font = new Font("Arial", fontSize))
            {
                Brush brush = Brushes.Black;
                for (int i = 0; i < BoardSize; i++)
                {
                    string colLabel = ((char)('A' + i)).ToString();
                    g.DrawString(colLabel, font, brush, i * SquareSize + SquareSize / 2f - fontSize, BoardSize * SquareSize);
                    string rowLabel = (BoardSize - i).ToString();
                    g.DrawString(rowLabel, font, brush, BoardSize * SquareSize, i * SquareSize + SquareSize / 2f - fontSize);
                }
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

        // Highlights the king's square in red when in check.
        public void DrawCheck(Graphics g, string[,] positions, bool whiteKingInCheck, bool blackKingInCheck)
        {
            if (!whiteKingInCheck && !blackKingInCheck) return;

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(160, Color.Red)))
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    for (int col = 0; col < BoardSize; col++)
                    {
                        string piece = positions[row, col];
                        if (piece == "K" && whiteKingInCheck)
                            g.FillRectangle(brush, col * SquareSize, row * SquareSize, SquareSize, SquareSize);
                        else if (piece == "k" && blackKingInCheck)
                            g.FillRectangle(brush, col * SquareSize, row * SquareSize, SquareSize, SquareSize);
                    }
                }
            }
        }

        // Displays the current players turn below the board.
        public void DrawTurnIndicator(Graphics g, string currentTurn)
        {
            string text = currentTurn == "white" ? "White's turn" : "Black's turn";
            Color color = currentTurn == "white" ? Color.White : Color.Black;
            currentTurnIsWhite = currentTurn == "white" ? true : false;

            float fontSize = Math.Max(10, SquareSize / 8f);
            using (Font font = new Font("Arial", fontSize, FontStyle.Bold))
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(180, Color.Gray)))
            using (SolidBrush fg = new SolidBrush(color))
            {
                int y = BoardSize * SquareSize + 10;
                g.FillRectangle(bg, 10, y - 4, 200, fontSize + 10);
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

            float pieceSize = Math.Max(16, SquareSize * 0.6f);
            float nudgeX = SquareSize * 0.04f;
            float nudgeY = SquareSize * 0.04f;

            using (Font font = new Font("Segoe UI Symbol", pieceSize))
            using (Font specialFont = new Font("Arial", pieceSize, FontStyle.Bold))
            {
                for (int row = 0; row < BoardSize; row++)
                {
                    for (int col = 0; col < BoardSize; col++)
                    {
                        string piece = positions[row, col];
                        if (!string.IsNullOrEmpty(piece))
                        {
                            Brush brush = char.IsUpper(piece[0]) ? Brushes.White : Brushes.Black;
                            Brush mateBrush = Brushes.Red;

                            bool isKing = piece == "K" || piece == "k";
                            bool isCheckmatedKing = (piece == "K" && !currentTurnIsWhite) || (piece == "k" && currentTurnIsWhite);

                            string drawSymbol;
                            Font drawFont;
                            Brush drawBrush;

                            if (isCheckmate && isCheckmatedKing)
                            {
                                drawSymbol = "#";
                                drawFont = specialFont;
                                drawBrush = mateBrush;
                            }
                            else if (isKing && isStalemate)
                            {
                                drawSymbol = "½";
                                drawFont = specialFont;
                                drawBrush = mateBrush;
                            }
                            else
                            {
                                drawSymbol = symbols[piece];
                                drawFont = font;
                                drawBrush = brush;
                            }

                            // Measure the symbol and center it within the square, with a small nudge
                            SizeF size = g.MeasureString(drawSymbol, drawFont);
                            float x = col * SquareSize + (SquareSize - size.Width) / 2f + nudgeX;
                            float y = row * SquareSize + (SquareSize - size.Height) / 2f + nudgeY;

                            g.DrawString(drawSymbol, drawFont, drawBrush, x, y);
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