using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess_game_2
{
    internal class Promotion
    {
        // Check if a pawn has reached the opposite end of the board and promote it to a piece chosen by the player.
        public void CheckAndPromote(string[,] positions, int row, int col)
        {
            // Check if the piece is a pawn and has reached the promotion rank
            string piece = positions[row, col];

            if (piece == "P" && row == 0)
                positions[row, col] = ShowDialog(true);
            else if (piece == "p" && row == 7)
                positions[row, col] = ShowDialog(false);
        }

        // Dialog to let the player choose what to promote to.
        private string ShowDialog(bool isWhite)
        {
            string[] options = isWhite
                ? new[] { "Q", "R", "B", "N" }
                : new[] { "q", "r", "b", "n" };

            string result = options[0];

            Form dialog = new Form();
            dialog.Text = "Promote pawn";
            dialog.Width = 300;
            dialog.Height = 100;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.StartPosition = FormStartPosition.CenterScreen;

            int x = 10;

            foreach (string option in options)
            {
                string capture = option;
                Button btn = new Button();
                btn.Text = option.ToUpper();
                btn.Width = 60;
                btn.Left = x;
                btn.Top = 20;
                btn.Click += (s, e) => { result = capture; dialog.Close(); };
                dialog.Controls.Add(btn);
                x += 65;
            }

            // Show the dialog and wait for the player to choose an option
            dialog.ShowDialog();
            return result;
        }
    }
}
