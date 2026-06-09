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

        public bool CurrentTurnIsWhite { get; set; } = true;

        // Board flip state
        public bool FlipBoardEnabled { get; set; } = false;

        Color DarkSquare = Color.DarkGreen;
        Color LightSquare = Color.LightSteelBlue;

        public static void RecalculateSquareSize(int availableHeight, int availableWidth)
        {
            int margin = 20;
            int maxByHeight = (availableHeight - margin - 50) / BoardSize;
            int maxByWidth = (availableWidth - margin - 50) / BoardSize;
            SquareSize = Math.Max(40, Math.Min(maxByHeight, maxByWidth));
        }

        public static void RecalculateSquareSizeFromScreen()
        {
            var screen = Screen.PrimaryScreen.WorkingArea;
            RecalculateSquareSize(screen.Height, screen.Width);
        }

        // Returns whether the board should currently be rendered flipped
        private bool IsFlipped => FlipBoardEnabled && !CurrentTurnIsWhite;

        // Converts a logical board row/col to a screen row/col.
        private int ToScreenRow(int logicalRow) => IsFlipped ? (BoardSize - 1 - logicalRow) : logicalRow;
        private int ToScreenCol(int logicalCol) => IsFlipped ? (BoardSize - 1 - logicalCol) : logicalCol;

        // Converts a screen pixel position back to a logical board row/col.
        public int ToLogicalRow(int screenRow) => IsFlipped ? (BoardSize - 1 - screenRow) : screenRow;
        public int ToLogicalCol(int screenCol) => IsFlipped ? (BoardSize - 1 - screenCol) : screenCol;

        // Draws the chessboard with alternating colors.
        public void DrawBoard(Graphics g)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Color squareColor = ((row + col) % 2 == 0) ? LightSquare : DarkSquare;
                    int screenRow = ToScreenRow(row);
                    int screenCol = ToScreenCol(col);
                    using (SolidBrush brush = new SolidBrush(squareColor))
                    {
                        g.FillRectangle(brush, screenCol * SquareSize, screenRow * SquareSize, SquareSize, SquareSize);
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
                    // File labels (A-H): flip horizontally when board is flipped
                    int fileIndex = IsFlipped ? (BoardSize - 1 - i) : i;
                    string colLabel = ((char)('A' + fileIndex)).ToString();
                    g.DrawString(colLabel, font, brush, i * SquareSize + SquareSize / 2f - fontSize, BoardSize * SquareSize);

                    // Rank labels (1-8): flip vertically when board is flipped
                    int rankNumber = IsFlipped ? (i + 1) : (BoardSize - i);
                    string rowLabel = rankNumber.ToString();
                    g.DrawString(rowLabel, font, brush, BoardSize * SquareSize, i * SquareSize + SquareSize / 2f - fontSize);
                }
            }
        }

        // Highlights the selected piece's square.
        public void DrawSelection(Graphics g, Pieces pieces)
        {
            if (!pieces.IsSelected) return;

            int screenRow = ToScreenRow(pieces.SelectedRow);
            int screenCol = ToScreenCol(pieces.SelectedCol);

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(120, Color.Yellow)))
            {
                g.FillRectangle(brush, screenCol * SquareSize, screenRow * SquareSize, SquareSize, SquareSize);
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
                        if ((piece == "K" && whiteKingInCheck) || (piece == "k" && blackKingInCheck))
                        {
                            int screenRow = ToScreenRow(row);
                            int screenCol = ToScreenCol(col);
                            g.FillRectangle(brush, screenCol * SquareSize, screenRow * SquareSize, SquareSize, SquareSize);
                        }
                    }
                }
            }
        }

        // Displays the current player's turn below the board.
        public void DrawTurnIndicator(Graphics g, string currentTurn)
        {
            string text = currentTurn == "white" ? "White's turn" : "Black's turn";
            Color color = currentTurn == "white" ? Color.White : Color.Black;
            CurrentTurnIsWhite = currentTurn == "white";

            float fontSize = Math.Max(10, SquareSize / 8f);
            using (Font font = new Font("Arial", fontSize, FontStyle.Bold))
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(180, Color.Gray)))
            using (SolidBrush fg = new SolidBrush(color))
            {
                int y = BoardSize * SquareSize + 10;
                g.FillRectangle(bg, 10, y + 10, 200, fontSize + 10);
                g.DrawString(text, font, fg, 15, y + 10);
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
                // Loops through the entire array and draws the symbols.
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
                            bool isCheckmatedKing = (piece == "K" && CurrentTurnIsWhite) || (piece == "k" && !CurrentTurnIsWhite);

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

                            int screenRow = ToScreenRow(row);
                            int screenCol = ToScreenCol(col);

                            SizeF size = g.MeasureString(drawSymbol, drawFont);
                            float x = screenCol * SquareSize + (SquareSize - size.Width) / 2f + nudgeX;
                            float y = screenRow * SquareSize + (SquareSize - size.Height) / 2f + nudgeY;

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
                int screenRow = ToScreenRow(row);
                int screenCol = ToScreenCol(col);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(140, Color.LimeGreen)))
                {
                    g.FillRectangle(brush, screenCol * SquareSize, screenRow * SquareSize, SquareSize, SquareSize);
                }
            }
        }
    }
}