using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess_game_2
{
    internal class Pieces
    {
        // Array that stores the current positions of pieces on the board.
        public string[,] positions = new string[8, 8]
        {
            { "r", "n", "b", "q", "k", "b", "n", "r" },
            { "p", "p", "p", "p", "p", "p", "p", "p" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "P", "P", "P", "P", "P", "P", "P", "P" },
            { "R", "N", "B", "Q", "K", "B", "N", "R" }
        };

        //Variables
        private bool pieceSelected = false;
        private int selectedRow;
        private int selectedCol;
        private string currentTurn = "white";

        public bool IsSelected => pieceSelected;
        public int SelectedRow => selectedRow;
        public int SelectedCol => selectedCol;
        public string CurrentTurn => currentTurn;

        private bool IsWhitePiece(string piece) => piece != "" && char.IsUpper(piece[0]);
        private bool IsBlackPiece(string piece) => piece != "" && char.IsLower(piece[0]);

        // Checks if the piece belongs to the player whose turn it is.
        private bool BelongsToCurrentTurn(string piece)
        {
            if (currentTurn == "white") return IsWhitePiece(piece);
            if (currentTurn == "black") return IsBlackPiece(piece);
            return false;
        }

        // Switches the turn to the other player.
        private void SwitchTurn()
        {
            currentTurn = (currentTurn == "white") ? "black" : "white";
        }

        public void HandleClick(int row, int col)
        {
            // Ignore clicks outside the board
            if (row < 0 || row > 7 || col < 0 || col > 7) return;

            // Get the piece at the clicked position
            string clicked = positions[row, col];

            // If no piece is currently selected, try to select one
            if (!pieceSelected)
            {
                if (clicked != "" && BelongsToCurrentTurn(clicked))
                {
                    selectedRow = row;
                    selectedCol = col;
                    pieceSelected = true;
                    validMoves = GetValidMovesFor(row, col);
                }
                return;
            }

            // If the same piece is clicked again, deselect it
            if (selectedRow == row && selectedCol == col)
            {
                pieceSelected = false;
                validMoves = null;
                return;
            }

            // If a different piece is clicked, check if it belongs to the current player and select it
            if (BelongsToCurrentTurn(clicked))
            {
                selectedRow = row;
                selectedCol = col;
                validMoves = GetValidMovesFor(row, col);
                return;
            }

            // Only move if destination is in valid moves
            if (validMoves != null && validMoves.Contains((row, col)))
            {
                string movingPiece = positions[selectedRow, selectedCol];

                enPassant.TryCapture(positions, movingPiece, row, col);
                castling.TryCastle(positions, movingPiece, selectedCol, row, col);

                positions[row, col] = movingPiece;
                positions[selectedRow, selectedCol] = "";

                // Notify castling and en passant trackers about the move so they can update their state.
                castling.NotifyPieceMoved(movingPiece, selectedRow, selectedCol);
                enPassant.Update(movingPiece, selectedRow, row, col);

                // Check for promotion after the move is made.
                promotion.CheckAndPromote(positions, row, col);

                pieceSelected = false;
                validMoves = null;
                SwitchTurn();
            }
        }

        // Determines valid moves for the piece at the given position.
        public List<(int row, int col)> GetValidMovesFor(int row, int col)
        {
            string piece = positions[row, col];
            if (piece == "") return new List<(int, int)>();

            string pieceType = piece.ToLower();

            // Use the appropriate move strategy based on the piece type.
            if (pieceType == "p") return new PawnMoves(enPassant.Row, enPassant.Col).GetValidMoves(row, col, positions);
            if (pieceType == "r") return new RookMoves().GetValidMoves(row, col, positions);
            if (pieceType == "n") return new KnightMoves().GetValidMoves(row, col, positions);
            if (pieceType == "b") return new BishopMoves().GetValidMoves(row, col, positions);
            if (pieceType == "q") return new QueenMoves().GetValidMoves(row, col, positions);
            if (pieceType == "k") return new KingMoves(castling).GetValidMoves(row, col, positions);


            return new List<(int, int)>();
        }

        // Store valid moves for the currently selected piece to highlight them on the board.
        private List<(int row, int col)> validMoves = null;
        public List<(int row, int col)> ValidMoves => validMoves;
        private EnPassant enPassant = new EnPassant();
        private Castling castling = new Castling();
        private Promotion promotion = new Promotion();
    }
}