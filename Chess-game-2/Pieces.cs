using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess_game_2
{
    internal class Pieces
    {
        public string[,] positions = new string[8, 8]
        {
            { "r", "n", "b", "q", "k", "b", "n", "r" },
            { "p", "p", "p", "p", "p", "p", "p", "p" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "P", "P", "P", "P", "P", "P", "P", "P" },
            { "R", "N", "B", "Q", "K", "B", "N", "R" }
        };

        private bool pieceSelected = false;
        private int selectedRow;
        private int selectedCol;
        private string currentTurn = "white";

        public bool IsSelected => pieceSelected;
        public int SelectedRow => selectedRow;
        public int SelectedCol => selectedCol;
        public string CurrentTurn => currentTurn;

        private bool IsWhitePiece(string piece) => piece != "" && char.IsUpper(piece[0]);
        private bool IsBlackPiece(string piece) => piece != "" && char.IsLower(piece[0]);

        private bool BelongsToCurrentTurn(string piece)
        {
            if (currentTurn == "white") return IsWhitePiece(piece);
            if (currentTurn == "black") return IsBlackPiece(piece);
            return false;
        }

        private void SwitchTurn()
        {
            currentTurn = (currentTurn == "white") ? "black" : "white";
        }

        public void HandleClick(int row, int col)
        {
            if (row < 0 || row > 7 || col < 0 || col > 7)
                return;

            string clicked = positions[row, col];

            if (!pieceSelected)
            {
                if (clicked != "" && BelongsToCurrentTurn(clicked))
                {
                    selectedRow = row;
                    selectedCol = col;
                    pieceSelected = true;
                }
                return;
            }

            if (selectedRow == row && selectedCol == col)
            {
                pieceSelected = false;
                return;
            }

            if (BelongsToCurrentTurn(clicked))
            {
                selectedRow = row;
                selectedCol = col;
                return;
            }

            positions[row, col] = positions[selectedRow, selectedCol];
            positions[selectedRow, selectedCol] = "";
            pieceSelected = false;

            SwitchTurn();
        }
    }
}