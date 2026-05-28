using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    // King moves: one square in any direction
    internal class KingMoves : IMoveStrategy
    {
        // Reference to the Castling class
        private Castling castling;

        // Check for castling moves
        public KingMoves(Castling castling)
        {
            this.castling = castling;
        }

        // A king can move one square in any direction, but cannot move into check
        public List<(int row, int col)> GetValidMoves(int row, int col, string[,] positions)
        {
            var moves = new List<(int, int)>();
            bool isWhite = char.IsUpper(positions[row, col][0]);

            int[,] directions = {
                { -1, -1 }, { -1, 0 }, { -1, 1 },
                {  0, -1 },             {  0, 1 },
                {  1, -1 }, {  1, 0 }, {  1, 1 }
            };

            // Check each of the 8 possible king moves
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int r = row + directions[i, 0];
                int c = col + directions[i, 1];

                if (MoveHelper.InBounds(r, c) && !MoveHelper.IsFriendly(r, c, positions, isWhite))
                    moves.Add((r, c));
            }

            // Add castling moves if available
            if (castling != null)
                moves.AddRange(castling.GetCastlingMoves(isWhite, positions));

            return moves;
        }
    }
}
