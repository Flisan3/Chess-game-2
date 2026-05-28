using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    // Knight moves: L-shaped jumps
    internal class KnightMoves : IMoveStrategy
    {
        // A knight can jump to any square that is either empty or occupied by an opponents piece
        public List<(int row, int col)> GetValidMoves(int row, int col, string[,] positions)
        {
            var moves = new List<(int, int)>();
            bool isWhite = char.IsUpper(positions[row, col][0]);

            int[,] jumps = {
                { -2, -1 }, { -2, 1 },
                { -1, -2 }, { -1, 2 },
                {  1, -2 }, {  1, 2 },
                {  2, -1 }, {  2, 1 }
            };

            // Check each of the 8 possible knight jumps
            for (int i = 0; i < jumps.GetLength(0); i++)
            {
                int r = row + jumps[i, 0];
                int c = col + jumps[i, 1];

                if (MoveHelper.InBounds(r, c) && !MoveHelper.IsFriendly(r, c, positions, isWhite))
                    moves.Add((r, c));
            }

            return moves;
        }
    }
}