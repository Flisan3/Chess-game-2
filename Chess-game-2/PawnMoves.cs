using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    // Pawn moves: one step forward, two steps from starting position, and diagonal captures
    internal class PawnMoves : IMoveStrategy
    {
        // En passant target square for the current turn
        private int enPassantRow;
        private int enPassantCol;

        // Initialize en passant target square
        public PawnMoves(int enPassantRow, int enPassantCol)
        {
            this.enPassantRow = enPassantRow;
            this.enPassantCol = enPassantCol;
        }

        // Determine valid moves for a pawn based on its position and the board state
        public List<(int row, int col)> GetValidMoves(int row, int col, string[,] positions)
        {
            var moves = new List<(int, int)>();
            bool isWhite = char.IsUpper(positions[row, col][0]);
            int direction = isWhite ? -1 : 1;
            int startRow = isWhite ? 6 : 1;

            // Move one step forward if the square is empty
            int nextRow = row + direction;
            if (MoveHelper.InBounds(nextRow, col) && MoveHelper.IsEmpty(nextRow, col, positions))
            {
                moves.Add((nextRow, col));

                // Move two steps forward from the starting position if both squares are empty
                int twoAhead = row + direction * 2;
                if (row == startRow && MoveHelper.IsEmpty(twoAhead, col, positions))
                    moves.Add((twoAhead, col));
            }

            // Check for diagonal captures
            foreach (int dc in new[] { -1, 1 })
            {
                int captureCol = col + dc;
                if (!MoveHelper.InBounds(nextRow, captureCol))
                    continue;

                // Normal capture
                if (MoveHelper.IsEnemy(nextRow, captureCol, positions, isWhite))
                {
                    moves.Add((nextRow, captureCol));
                }

                // En passant capture
                if (nextRow == enPassantRow && captureCol == enPassantCol)
                {
                    moves.Add((nextRow, captureCol));
                }
            }

            return moves;
        }
    }
}
