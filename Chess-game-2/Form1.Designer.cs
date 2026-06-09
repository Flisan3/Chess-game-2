namespace Chess_game_2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flipCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // flipCheckBox
            // 
            this.flipCheckBox.AutoSize = true;
            this.flipCheckBox.Location = new System.Drawing.Point(249, 1008);
            this.flipCheckBox.Name = "flipCheckBox";
            this.flipCheckBox.Size = new System.Drawing.Size(112, 17);
            this.flipCheckBox.TabIndex = 0;
            this.flipCheckBox.Text = "Automatic Flipping";
            this.flipCheckBox.UseVisualStyleBackColor = true;
            this.flipCheckBox.CheckedChanged += new System.EventHandler(this.flipCheckBox_CheckedChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 1088);
            this.Controls.Add(this.flipCheckBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox flipCheckBox;
    }
}

