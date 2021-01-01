namespace ChessEngine
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
            this.authTokenTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.getAcctInfo = new System.Windows.Forms.Button();
            this.gamesListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.boardTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.executeUCITxtBox = new System.Windows.Forms.TextBox();
            this.executeUCIBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // authTokenTxtBox
            // 
            this.authTokenTxtBox.Location = new System.Drawing.Point(106, 22);
            this.authTokenTxtBox.Name = "authTokenTxtBox";
            this.authTokenTxtBox.Size = new System.Drawing.Size(231, 20);
            this.authTokenTxtBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "OAuth Token:";
            // 
            // getAcctInfo
            // 
            this.getAcctInfo.Location = new System.Drawing.Point(366, 22);
            this.getAcctInfo.Name = "getAcctInfo";
            this.getAcctInfo.Size = new System.Drawing.Size(128, 23);
            this.getAcctInfo.TabIndex = 2;
            this.getAcctInfo.Text = "Load Account Info";
            this.getAcctInfo.UseVisualStyleBackColor = true;
            this.getAcctInfo.Click += new System.EventHandler(this.getAcctInfo_Click);
            // 
            // gamesListBox
            // 
            this.gamesListBox.FormattingEnabled = true;
            this.gamesListBox.Location = new System.Drawing.Point(29, 100);
            this.gamesListBox.Name = "gamesListBox";
            this.gamesListBox.Size = new System.Drawing.Size(232, 329);
            this.gamesListBox.TabIndex = 3;
            this.gamesListBox.SelectedIndexChanged += new System.EventHandler(this.gamesListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Active Games";
            // 
            // boardTextBox
            // 
            this.boardTextBox.Location = new System.Drawing.Point(333, 100);
            this.boardTextBox.Multiline = true;
            this.boardTextBox.Name = "boardTextBox";
            this.boardTextBox.Size = new System.Drawing.Size(455, 290);
            this.boardTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(333, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Execute UCI:";
            // 
            // executeUCITxtBox
            // 
            this.executeUCITxtBox.Location = new System.Drawing.Point(409, 394);
            this.executeUCITxtBox.Name = "executeUCITxtBox";
            this.executeUCITxtBox.Size = new System.Drawing.Size(278, 20);
            this.executeUCITxtBox.TabIndex = 7;
            this.executeUCITxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.executeUCITxtBox_KeyDown);
            // 
            // executeUCIBtn
            // 
            this.executeUCIBtn.Location = new System.Drawing.Point(693, 392);
            this.executeUCIBtn.Name = "executeUCIBtn";
            this.executeUCIBtn.Size = new System.Drawing.Size(95, 23);
            this.executeUCIBtn.TabIndex = 8;
            this.executeUCIBtn.Text = "Execute";
            this.executeUCIBtn.UseVisualStyleBackColor = true;
            this.executeUCIBtn.Click += new System.EventHandler(this.executeUCIBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 477);
            this.Controls.Add(this.executeUCIBtn);
            this.Controls.Add(this.executeUCITxtBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.boardTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gamesListBox);
            this.Controls.Add(this.getAcctInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.authTokenTxtBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox authTokenTxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button getAcctInfo;
        private System.Windows.Forms.ListBox gamesListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox boardTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox executeUCITxtBox;
        private System.Windows.Forms.Button executeUCIBtn;
    }
}

