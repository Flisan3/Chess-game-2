using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Chess_game_2
{
    // En passant: Special side by side pawn capture after an opponents pawn moves two squares.
    internal class EnPassant
    {
        // Track the position of the pawn that can be captured en passant
        private int enPassantRow = -1;
        private int enPassantCol = -1;
        public int Row => enPassantRow;
        public int Col => enPassantCol;

        // Update en passant target square after a pawn moves. Only set if a pawn moves two squares forward.
        public void Update(string piece, int fromRow, int toRow, int toCol)
        {
            if ((piece == "P" || piece == "p") && System.Math.Abs(toRow - fromRow) == 2)
            {
                enPassantRow = (fromRow + toRow) / 2;
                enPassantCol = toCol;
            }
            else
            {
                enPassantRow = -1;
                enPassantCol = -1;
            }
        }

        // If the move is an en passant capture, remove the captured pawn from the board.
        public void TryCapture(string[,] positions, string piece, int toRow, int toCol)
        {
            if ((piece == "P" || piece == "p") && toCol == enPassantCol && toRow == enPassantRow)
            {
                int capturedRow = piece == "P" ? toRow + 1 : toRow - 1;
                positions[capturedRow, toCol] = "";
            }
        }
    }
}
