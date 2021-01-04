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
            this.label3 = new System.Windows.Forms.Label();
            this.executeUCITxtBox = new System.Windows.Forms.TextBox();
            this.executeUCIBtn = new System.Windows.Forms.Button();
            this.ChessBoardPlaceholder = new System.Windows.Forms.GroupBox();
            this.turnLbl = new System.Windows.Forms.Label();
            this.playerClockNameLbl = new System.Windows.Forms.Label();
            this.playerClockTimeLbl = new System.Windows.Forms.Label();
            this.oppClockTimeLbl = new System.Windows.Forms.Label();
            this.oppClockNameLbl = new System.Windows.Forms.Label();
            this.oppBoardNameLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // authTokenTxtBox
            // 
            this.authTokenTxtBox.Location = new System.Drawing.Point(94, 14);
            this.authTokenTxtBox.Name = "authTokenTxtBox";
            this.authTokenTxtBox.Size = new System.Drawing.Size(161, 20);
            this.authTokenTxtBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "OAuth Token:";
            // 
            // getAcctInfo
            // 
            this.getAcctInfo.Location = new System.Drawing.Point(261, 12);
            this.getAcctInfo.Name = "getAcctInfo";
            this.getAcctInfo.Size = new System.Drawing.Size(106, 23);
            this.getAcctInfo.TabIndex = 2;
            this.getAcctInfo.Text = "Load Account Info";
            this.getAcctInfo.UseVisualStyleBackColor = true;
            this.getAcctInfo.Click += new System.EventHandler(this.getAcctInfo_Click);
            // 
            // gamesListBox
            // 
            this.gamesListBox.FormattingEnabled = true;
            this.gamesListBox.Location = new System.Drawing.Point(17, 102);
            this.gamesListBox.Name = "gamesListBox";
            this.gamesListBox.Size = new System.Drawing.Size(367, 186);
            this.gamesListBox.TabIndex = 3;
            this.gamesListBox.SelectedIndexChanged += new System.EventHandler(this.gamesListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Active Games";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Execute UCI:";
            // 
            // executeUCITxtBox
            // 
            this.executeUCITxtBox.Location = new System.Drawing.Point(94, 51);
            this.executeUCITxtBox.Name = "executeUCITxtBox";
            this.executeUCITxtBox.Size = new System.Drawing.Size(161, 20);
            this.executeUCITxtBox.TabIndex = 7;
            this.executeUCITxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.executeUCITxtBox_KeyDown);
            // 
            // executeUCIBtn
            // 
            this.executeUCIBtn.Location = new System.Drawing.Point(261, 49);
            this.executeUCIBtn.Name = "executeUCIBtn";
            this.executeUCIBtn.Size = new System.Drawing.Size(106, 23);
            this.executeUCIBtn.TabIndex = 8;
            this.executeUCIBtn.Text = "Execute";
            this.executeUCIBtn.UseVisualStyleBackColor = true;
            this.executeUCIBtn.Click += new System.EventHandler(this.executeUCIBtn_Click);
            // 
            // ChessBoardPlaceholder
            // 
            this.ChessBoardPlaceholder.Location = new System.Drawing.Point(794, 81);
            this.ChessBoardPlaceholder.Name = "ChessBoardPlaceholder";
            this.ChessBoardPlaceholder.Size = new System.Drawing.Size(800, 800);
            this.ChessBoardPlaceholder.TabIndex = 10;
            this.ChessBoardPlaceholder.TabStop = false;
            this.ChessBoardPlaceholder.Text = "groupBox1";
            this.ChessBoardPlaceholder.Visible = false;
            // 
            // turnLbl
            // 
            this.turnLbl.AutoSize = true;
            this.turnLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.turnLbl.Location = new System.Drawing.Point(1100, 9);
            this.turnLbl.Name = "turnLbl";
            this.turnLbl.Size = new System.Drawing.Size(0, 55);
            this.turnLbl.TabIndex = 11;
            this.turnLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playerClockNameLbl
            // 
            this.playerClockNameLbl.AutoSize = true;
            this.playerClockNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerClockNameLbl.Location = new System.Drawing.Point(400, 102);
            this.playerClockNameLbl.Name = "playerClockNameLbl";
            this.playerClockNameLbl.Size = new System.Drawing.Size(108, 16);
            this.playerClockNameLbl.TabIndex = 12;
            this.playerClockNameLbl.Text = "Player\'s Clock";
            // 
            // playerClockTimeLbl
            // 
            this.playerClockTimeLbl.AutoSize = true;
            this.playerClockTimeLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerClockTimeLbl.Location = new System.Drawing.Point(403, 131);
            this.playerClockTimeLbl.Name = "playerClockTimeLbl";
            this.playerClockTimeLbl.Size = new System.Drawing.Size(97, 25);
            this.playerClockTimeLbl.TabIndex = 13;
            this.playerClockTimeLbl.Text = "000:000";
            this.playerClockTimeLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // oppClockTimeLbl
            // 
            this.oppClockTimeLbl.AutoSize = true;
            this.oppClockTimeLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oppClockTimeLbl.Location = new System.Drawing.Point(403, 195);
            this.oppClockTimeLbl.Name = "oppClockTimeLbl";
            this.oppClockTimeLbl.Size = new System.Drawing.Size(97, 25);
            this.oppClockTimeLbl.TabIndex = 15;
            this.oppClockTimeLbl.Text = "000:000";
            this.oppClockTimeLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // oppClockNameLbl
            // 
            this.oppClockNameLbl.AutoSize = true;
            this.oppClockNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oppClockNameLbl.Location = new System.Drawing.Point(400, 166);
            this.oppClockNameLbl.Name = "oppClockNameLbl";
            this.oppClockNameLbl.Size = new System.Drawing.Size(130, 16);
            this.oppClockNameLbl.TabIndex = 14;
            this.oppClockNameLbl.Text = "Opponent\'s Clock";
            // 
            // oppBoardNameLbl
            // 
            this.oppBoardNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oppBoardNameLbl.Location = new System.Drawing.Point(17, 305);
            this.oppBoardNameLbl.Name = "oppBoardNameLbl";
            this.oppBoardNameLbl.Size = new System.Drawing.Size(367, 23);
            this.oppBoardNameLbl.TabIndex = 16;
            this.oppBoardNameLbl.Text = "Opponent\'s Board";
            this.oppBoardNameLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 369);
            this.Controls.Add(this.oppBoardNameLbl);
            this.Controls.Add(this.oppClockTimeLbl);
            this.Controls.Add(this.oppClockNameLbl);
            this.Controls.Add(this.playerClockTimeLbl);
            this.Controls.Add(this.playerClockNameLbl);
            this.Controls.Add(this.turnLbl);
            this.Controls.Add(this.ChessBoardPlaceholder);
            this.Controls.Add(this.executeUCIBtn);
            this.Controls.Add(this.executeUCITxtBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gamesListBox);
            this.Controls.Add(this.getAcctInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.authTokenTxtBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox executeUCITxtBox;
        private System.Windows.Forms.Button executeUCIBtn;
        private System.Windows.Forms.GroupBox ChessBoardPlaceholder;
        private System.Windows.Forms.Label turnLbl;
        private System.Windows.Forms.Label playerClockNameLbl;
        private System.Windows.Forms.Label playerClockTimeLbl;
        private System.Windows.Forms.Label oppClockTimeLbl;
        private System.Windows.Forms.Label oppClockNameLbl;
        private System.Windows.Forms.Label oppBoardNameLbl;
    }
}

