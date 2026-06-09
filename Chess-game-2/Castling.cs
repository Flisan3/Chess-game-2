using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    internal class Castling
    {
        // Track whether the king and rooks have moved to determine castling rights
        private bool whiteKingMoved = false;
        private bool blackKingMoved = false;
        private bool whiteRookKingsideMoved = false;
        private bool whiteRookQueensideMoved = false;
        private bool blackRookKingsideMoved = false;
        private bool blackRookQueensideMoved = false;

        // Method to update castling rights
        public void NotifyPieceMoved(string piece, int fromRow, int fromCol)
        {
            if (piece == "K") whiteKingMoved = true;
            if (piece == "k") blackKingMoved = true;
            if (piece == "R" && fromRow == 7 && fromCol == 7) whiteRookKingsideMoved = true;
            if (piece == "R" && fromRow == 7 && fromCol == 0) whiteRookQueensideMoved = true;
            if (piece == "r" && fromRow == 0 && fromCol == 7) blackRookKingsideMoved = true;
            if (piece == "r" && fromRow == 0 && fromCol == 0) blackRookQueensideMoved = true;
        }

        // Method to get possible castling moves for the king
        public List<(int row, int col)> GetCastlingMoves(bool isWhite, string[,] positions)
        {
            var moves = new List<(int, int)>();

            if (isWhite && !whiteKingMoved)
            {
                // Kingside
                if (!whiteRookKingsideMoved &&
                    positions[7, 5] == "" && positions[7, 6] == "")
                    moves.Add((7, 6));

                // Queenside
                if (!whiteRookQueensideMoved &&
                    positions[7, 3] == "" && positions[7, 2] == "" && positions[7, 1] == "")
                    moves.Add((7, 2));
            }

            if (!isWhite && !blackKingMoved)
            {
                // Kingside
                if (!blackRookKingsideMoved &&
                    positions[0, 5] == "" && positions[0, 6] == "")
                    moves.Add((0, 6));

                // Queenside
                if (!blackRookQueensideMoved &&
                    positions[0, 3] == "" && positions[0, 2] == "" && positions[0, 1] == "")
                    moves.Add((0, 2));
            }

            return moves;
        }

        // Method to move the rook
        public void TryCastle(string[,] positions, string piece, int fromCol, int toRow, int toCol)
        {
            if (piece != "K" && piece != "k") return;

            // White kingside
            if (piece == "K" && fromCol == 4 && toCol == 6)
            {
                positions[7, 5] = "R";
                positions[7, 7] = "";
            }
            // White queenside
            else if (piece == "K" && fromCol == 4 && toCol == 2)
            {
                positions[7, 3] = "R";
                positions[7, 0] = "";
            }
            // Black kingside
            else if (piece == "k" && fromCol == 4 && toCol == 6)
            {
                positions[0, 5] = "r";
                positions[0, 7] = "";
            }
            // Black queenside
            else if (piece == "k" && fromCol == 4 && toCol == 2)
            {
                positions[0, 3] = "r";
                positions[0, 0] = "";
            }
        }
    }
}