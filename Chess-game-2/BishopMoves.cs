using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    // Bishop moves: diagonal sliding
    internal class BishopMoves : IMoveStrategy
    {
        // Directions: top-left, top-right, bottom-left, bottom-right
        public List<(int row, int col)> GetValidMoves(int row, int col, string[,] positions)
        {
            var moves = new List<(int, int)>();
            bool isWhite = char.IsUpper(positions[row, col][0]);

            int[,] directions = { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

            // Use helper to add sliding moves in each diagonal direction
            for (int i = 0; i < directions.GetLength(0); i++)
                MoveHelper.AddSlidingMoves(row, col, directions[i, 0], directions[i, 1], isWhite, positions, moves);

            return moves;
        }
    }
}
