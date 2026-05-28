using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_game_2
{
    // Effective way to store named variables
    internal enum GameState
    {
        Normal,
        Check,
        Checkmate,
        Stalemate
    }

    // Class to determine the current game state
    internal class GameStateChecker
    {
        // Reuse the check detector and move filter to avoid redundant code
        private CheckDetector checkDetector = new CheckDetector();
        private MoveFilter moveFilter = new MoveFilter();

        // Determine the game state based on whether the current player is in check and if they have any legal moves.
        public GameState GetState(string[,] positions, bool isWhiteTurn, EnPassant enPassant, Castling castling)
        {
            bool inCheck = checkDetector.IsInCheck(positions, isWhiteTurn);
            bool hasLegalMoves = HasAnyLegalMoves(positions, isWhiteTurn, enPassant, castling);

            if (inCheck && !hasLegalMoves) return GameState.Checkmate;
            if (!inCheck && !hasLegalMoves) return GameState.Stalemate;
            if (inCheck) return GameState.Check;
            return GameState.Normal;
        }

        private bool HasAnyLegalMoves(string[,] positions, bool isWhiteTurn, EnPassant enPassant, Castling castling)
        {
            // Runs trough all squares on the board
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    // Skip empty squares and opponent's pieces
                    string piece = positions[r, c];
                    if (piece == "") continue;
                    bool pieceIsWhite = char.IsUpper(piece[0]);
                    if (pieceIsWhite != isWhiteTurn) continue;

                    // Get candidate moves based on piece type
                    string pieceType = piece.ToLower();
                    List<(int, int)> candidates;

                    // Pass en passant and castling info to move generators that need it
                    if (pieceType == "p") candidates = new PawnMoves(enPassant.Row, enPassant.Col).GetValidMoves(r, c, positions);
                    else if (pieceType == "r") candidates = new RookMoves().GetValidMoves(r, c, positions);
                    else if (pieceType == "n") candidates = new KnightMoves().GetValidMoves(r, c, positions);
                    else if (pieceType == "b") candidates = new BishopMoves().GetValidMoves(r, c, positions);
                    else if (pieceType == "q") candidates = new QueenMoves().GetValidMoves(r, c, positions);
                    else if (pieceType == "k") candidates = new KingMoves(castling).GetValidMoves(r, c, positions);
                    else continue;

                    // Filter out moves that would leave the king in check
                    var legal = moveFilter.FilterLegalMoves(r, c, candidates, positions);
                    if (legal.Count > 0) return true;
                }
            }
            return false;
        }
    }
}
