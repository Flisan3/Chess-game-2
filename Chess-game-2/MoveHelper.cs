using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Chess_game_2
{
    // Methods for move validation
    internal static class MoveHelper
    {
        // Check if position is on the board
        public static bool InBounds(int row, int col)
            => row >= 0 && row <= 7 && col >= 0 && col <= 7;

        // Check if the target square is empty
        public static bool IsEmpty(int row, int col, string[,] positions)
            => positions[row, col] == "";

        // Check if the target square has an enemy piece
        public static bool IsEnemy(int row, int col, string[,] positions, bool isWhite)
        {
            string target = positions[row, col];
            if (target == "") return false;
            return isWhite ? char.IsLower(target[0]) : char.IsUpper(target[0]);
        }

        // Check if the target square has a friendly piece
        public static bool IsFriendly(int row, int col, string[,] positions, bool isWhite)
        {
            string target = positions[row, col];
            if (target == "") return false;
            return isWhite ? char.IsUpper(target[0]) : char.IsLower(target[0]);
        }

        // Helper for adding sliding moves like rook, bishop and queen.
        public static void AddSlidingMoves(
            int row, int col,
            int dRow, int dCol,
            bool isWhite,
            string[,] positions,
            List<(int, int)> moves)
        {
            int r = row + dRow;
            int c = col + dCol;

            // Keep moving in the direction until we hit a piece or go out of bounds
            while (InBounds(r, c))
            {
                if (IsEmpty(r, c, positions))
                {
                    moves.Add((r, c));
                }
                else
                {
                    if (IsEnemy(r, c, positions, isWhite))
                        moves.Add((r, c));
                    break;
                }
                r += dRow;
                c += dCol;
            }
        }
    }
}
