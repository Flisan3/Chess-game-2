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
        public bool WhiteKingInCheck => currentGameState == GameState.Check && currentTurn == "white";
        public bool BlackKingInCheck => currentGameState == GameState.Check && currentTurn == "black";

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

        // Moves the pieces
        public void HandleClick(int row, int col)
        {
            // Ignore clicks outside the board
            if (row < 0 || row > 7 || col < 0 || col > 7) return;

            string clicked = positions[row, col];

            // If no piece is currently selected, try to select the clicked piece.
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

            // If the clicked square is the currently selected piece, deselect it.
            if (selectedRow == row && selectedCol == col)
            {
                pieceSelected = false;
                validMoves = null;
                return;
            }

            // If the clicked piece belongs to the current player, select it instead.
            if (BelongsToCurrentTurn(clicked))
            {
                selectedRow = row;
                selectedCol = col;
                validMoves = GetValidMovesFor(row, col);
                return;
            }

            // If the clicked square is a valid move for the selected piece, move it.
            if (validMoves != null && validMoves.Contains((row, col)))
            {
                // Handle special moves like en passant and castling before moving the piece.
                string movingPiece = positions[selectedRow, selectedCol];

                enPassant.TryCapture(positions, movingPiece, row, col);
                castling.TryCastle(positions, movingPiece, selectedCol, row, col);

                positions[row, col] = movingPiece;
                positions[selectedRow, selectedCol] = "";

                castling.NotifyPieceMoved(movingPiece, selectedRow, selectedCol);
                enPassant.Update(movingPiece, selectedRow, row, col);

                promotion.CheckAndPromote(positions, row, col);

                pieceSelected = false;
                validMoves = null;
                SwitchTurn();
                currentGameState = gameStateChecker.GetState(positions, currentTurn == "white", enPassant, castling);
            }
        }

        // Determines valid moves for the piece at the given position.
        public List<(int row, int col)> GetValidMovesFor(int row, int col)
        {
            string piece = positions[row, col];
            if (piece == "") return new List<(int, int)>();

            string pieceType = piece.ToLower();
            List<(int, int)> candidates;

            // Get moves based on piece type, then filter them to ensure they don't put the king in check.
            if (pieceType == "p") candidates = new PawnMoves(enPassant.Row, enPassant.Col).GetValidMoves(row, col, positions);
            else if (pieceType == "r") candidates = new RookMoves().GetValidMoves(row, col, positions);
            else if (pieceType == "n") candidates = new KnightMoves().GetValidMoves(row, col, positions);
            else if (pieceType == "b") candidates = new BishopMoves().GetValidMoves(row, col, positions);
            else if (pieceType == "q") candidates = new QueenMoves().GetValidMoves(row, col, positions);
            else if (pieceType == "k") candidates = new KingMoves(castling).GetValidMoves(row, col, positions);
            else return new List<(int, int)>();

            return moveFilter.FilterLegalMoves(row, col, candidates, positions);
        }

        // Store valid moves for the currently selected piece to highlight them on the board.
        private List<(int row, int col)> validMoves = null;
        public List<(int row, int col)> ValidMoves => validMoves;
        private EnPassant enPassant = new EnPassant();
        private Castling castling = new Castling();
        private Promotion promotion = new Promotion();
        private MoveFilter moveFilter = new MoveFilter();
        private GameStateChecker gameStateChecker = new GameStateChecker();
        private GameState currentGameState = GameState.Normal;
        public GameState CurrentGameState => currentGameState;
    }
}