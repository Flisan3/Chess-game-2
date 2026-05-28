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
        public void CheckAndPromote(string[,] positions, int row, int col)
        {
            string piece = positions[row, col];

            if (piece == "P" && row == 0)
                positions[row, col] = ShowDialog(true);
            else if (piece == "p" && row == 7)
                positions[row, col] = ShowDialog(false);
        }

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

            dialog.ShowDialog();
            return result;
        }
    }
}
