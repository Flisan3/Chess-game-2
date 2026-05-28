using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    internal class CheckDetector
    {
        // Checks if the king is under attack by any opponent piece.
        public bool IsInCheck(string[,] positions, bool isWhite)
        {
            // Find the kings position
            int kingRow = -1, kingCol = -1;
            string kingSymbol = isWhite ? "K" : "k";

            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (positions[r, c] == kingSymbol)
                    { kingRow = r; kingCol = c; }

            // If king is not found
            if (kingRow == -1) return false;

            // Check if any opponent piece can move to the king's position
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    string piece = positions[r, c];
                    if (piece == "") continue;
                    bool pieceIsWhite = char.IsUpper(piece[0]);
                    if (pieceIsWhite == isWhite) continue;

                    var moves = GetRawMoves(r, c, positions);
                    if (moves.Contains((kingRow, kingCol)))
                        return true;
                }
            }

            // No opponent piece can attack the king
            return false;
        }

        // Get all possible moves for a piece to determine if the king is attacked.
        public List<(int row, int col)> GetRawMoves(int row, int col, string[,] positions)
        {
            string piece = positions[row, col];
            if (piece == "") return new List<(int, int)>();

            string pieceType = piece.ToLower();

            // Use the appropriate move strategy for each piece type
            if (pieceType == "p") return new PawnMoves(-1, -1).GetValidMoves(row, col, positions);
            if (pieceType == "r") return new RookMoves().GetValidMoves(row, col, positions);
            if (pieceType == "n") return new KnightMoves().GetValidMoves(row, col, positions);
            if (pieceType == "b") return new BishopMoves().GetValidMoves(row, col, positions);
            if (pieceType == "q") return new QueenMoves().GetValidMoves(row, col, positions);
            if (pieceType == "k") return new KingMoves(null).GetValidMoves(row, col, positions);

            return new List<(int, int)>();
        }
    }
}
