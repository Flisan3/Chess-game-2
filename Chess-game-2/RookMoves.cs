using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Chess_game_2
{
    // Rook moves: horizontal and vertical sliding
    internal class RookMoves : IMoveStrategy
    {
        // Directions: up, down, left, right
        public List<(int row, int col)> GetValidMoves(int row, int col, string[,] positions)
        {
            var moves = new List<(int, int)>();
            bool isWhite = char.IsUpper(positions[row, col][0]);

            int[,] directions = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            // Use helper to add sliding moves in each direction
            for (int i = 0; i < directions.GetLength(0); i++)
                MoveHelper.AddSlidingMoves(row, col, directions[i, 0], directions[i, 1], isWhite, positions, moves);

            return moves;
        }
    }
}
