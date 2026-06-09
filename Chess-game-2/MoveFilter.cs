using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    internal class MoveFilter
    {
        // Reuse the check detector
        private CheckDetector checkDetector = new CheckDetector();

        // Filters candidate moves to ensure they dont leave the king in check.
        public List<(int row, int col)> FilterLegalMoves(
            int fromRow, int fromCol,
            List<(int row, int col)> candidates,
            string[,] positions)
        {
            // Get the piece being moved and determine its color
            string piece = positions[fromRow, fromCol];
            bool isWhite = char.IsUpper(piece[0]);
            var legal = new List<(int, int)>();

            // Check if the piece being moved is the king to determine castling rules and check status
            bool isKing = piece == "K" || piece == "k";
            bool currentlyInCheck = isKing && checkDetector.IsInCheck(positions, isWhite);

            foreach (var (toRow, toCol) in candidates)
            {
                // If king is castling it moves 2 squares
                if (isKing && System.Math.Abs(toCol - fromCol) == 2)
                {
                    // Cannot castle while in check
                    if (currentlyInCheck) continue;

                    // Check that the square the king passes through is not attacked
                    int passThroughCol = fromCol + (toCol > fromCol ? 1 : -1);

                    string[,] passCopy = CopyBoard(positions);
                    passCopy[fromRow, passThroughCol] = passCopy[fromRow, fromCol];
                    passCopy[fromRow, fromCol] = "";

                    if (checkDetector.IsInCheck(passCopy, isWhite)) continue;
                }

                // Simulate the move and check if king is in check
                string[,] copy = CopyBoard(positions);
                copy[toRow, toCol] = copy[fromRow, fromCol];
                copy[fromRow, fromCol] = "";

                if (!checkDetector.IsInCheck(copy, isWhite))
                    legal.Add((toRow, toCol));
            }

            return legal;
        }

        // Helper method to create a copy of the board state
        public string[,] CopyBoard(string[,] positions)
        {
            string[,] copy = new string[8, 8];
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    copy[r, c] = positions[r, c];
            return copy;
        }
    }
}
