using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessEngine
{
    public partial class Form1 : Form
    {
        public Account currentAccount = null;
        private delegate void SafeCallDelegate();

        CheckBox[,] chessBoardList = new CheckBox[8, 8]; // Match BoardSpace model

        public Form1()
        {
            InitializeComponent();

            authTokenTxtBox.Text = "TzURxC7XfDjsGc5n";

        }

        private void boardSqaureCheckChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if(cb.Checked)
            {
                cb.Image = ResizeImage(Properties.Resources.BlackKnight,  cb.Size.Width/2, cb.Size.Height/2);
                cb.BackColor = Color.Yellow;
            }
            else
            {
                cb.Image = null;
                cb.BackColor = Color.White;
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void getAcctInfo_Click(object sender, EventArgs e)
        {
            if (currentAccount == null)
            {
                try
                {
                    currentAccount = new Account(authTokenTxtBox.Text.Trim());
                    getAcctInfo.Enabled = false;

                    updateGames();

                }
                catch(Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Cant Load Account");
                }
                
            }
            else
            {
                MessageBox.Show("Account Already Loaded","Account Loaded");
            }
        }

        private bool updateGames()
        {
            bool rv = false;

            if (currentAccount != null)
            {
                // Have Account request games Again
                currentAccount.getGames();
                gamesListBox.Items.Clear();
                foreach (Game g in currentAccount.currentGames)
                {

                    gamesListBox.Items.Add(g);
                }
            }

            return rv;
        }

        private void gamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox b = (ListBox)sender;
            // Set Browser
            if (b.SelectedIndex != -1)
            {
                Game currGame = ((Game)b.Items[b.SelectedIndex]);
                currGame.BoardUpdated += updateGUIAfterBoardChange;
                currGame.startGameStream();
                boardTextBox.Text = currGame.getBoard().printBoard(currGame.color);

                boardTextBox.Text += Environment.NewLine + ((Game)b.Items[b.SelectedIndex]).getBoard().printBoard((currGame.color.ToLower() == "white") ? "black" : "white") + Environment.NewLine;

                // Create Chess Board buttons
                // A1 is Bottom Left Corner, H8 is top Right Corner
                Array.Clear(chessBoardList, 0, chessBoardList.Length);

                if (currGame.color == "black")
                {
                    for (int row = 7; row >=0 ; row--)
                    {
                        int cbXDim = 100;
                        int cbYDim = 100;
                        // Initial Cb positon = 333x, 441y
                        int yPos = 441 + (cbYDim * row);
                        for (int col = 7; col >= 0; col--)
                        {
                            int xPos = 333 + (col * cbXDim);
                            Char tmp = (Char)('A' + col);
                            int effectiveRow = 8 - row;
                            //boardState[row, col] = new BoardSpace(tmp, effectiveRow);

                            CheckBox cb = new CheckBox();

                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.CheckedChanged += boardSqaureCheckChanged;
                            cb.Name = $"{tmp}{effectiveRow}";
                            if (effectiveRow == 1) // Use As Legend
                            {
                                // Set Column Legends
                            }
                            else if (col == 0) // Use As Legend
                            {
                                // Set Row Legends

                            }
                            else
                            {
                                cb.Text = cb.Name;
                            }

                            cb.Size = new Size(cbXDim, cbYDim);
                            cb.BackColor = Color.White;

                            cb.Location = new Point(xPos, yPos);

                            Controls.Add(cb);
                            chessBoardList[row, col] = cb;


                        }
                    }
                }
                else
                {
                    for (int row = 0; row < 8; row++)
                    {
                        int cbXDim = 100;
                        int cbYDim = 100;
                        // Initial Cb positon = 333x, 441y
                        int yPos = 441 + (cbYDim * row);
                        for (int col = 0; col < 8; col++)
                        {
                            int xPos = 333 + (col * cbXDim);
                            Char tmp = (Char)('A' + col);
                            int effectiveRow = 8 - row;
                            //boardState[row, col] = new BoardSpace(tmp, effectiveRow);

                            CheckBox cb = new CheckBox();

                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.CheckedChanged += boardSqaureCheckChanged;
                            cb.Name = $"{tmp}{effectiveRow}";
                            if (effectiveRow == 1) // Use As Legend
                            {
                                // Set Column Legends
                            }
                            else if (col == 0) // Use As Legend
                            {
                                // Set Row Legends

                            }
                            else
                            {
                                cb.Text = cb.Name;
                            }

                            cb.Size = new Size(cbXDim, cbYDim);
                            cb.BackColor = Color.White;

                            cb.Location = new Point(xPos, yPos);

                            Controls.Add(cb);
                            chessBoardList[row, col] = cb;


                        }
                    }
                }
                
            }
        }

        public void updateGUIAfterBoardChange()
        {
            if (this.InvokeRequired)
            {
                SafeCallDelegate d = new SafeCallDelegate(updateGUIAfterBoardChange);
                this.Invoke(d, new object[] {  });
            }
            else
            {
                if (gamesListBox.SelectedIndex != -1)
                {
                    Game currGame = ((Game)gamesListBox.Items[gamesListBox.SelectedIndex]);

                    boardTextBox.Text = currGame.getBoard().printBoard(currGame.color);
                    boardTextBox.Text += Environment.NewLine + ((Game)gamesListBox.Items[gamesListBox.SelectedIndex]).getBoard().printBoard((currGame.color.ToLower() == "white") ? "black" : "white") + Environment.NewLine;
                }
            }

        }

        private void executeUCIBtn_Click(object sender, EventArgs e)
        {
            // Execute UCI
            bool processedSuccessfully = false;
            if (gamesListBox.SelectedIndex >= 0)
            {
                try
                {
                    if (Regex.IsMatch(executeUCITxtBox.Text.Trim(), $"[abcdefgh][0-9][abcdefgh][0-9]"))
                    {
                        if (!((Game)gamesListBox.Items[gamesListBox.SelectedIndex]).makeMove(executeUCITxtBox.Text.Trim()))
                        {
                            MessageBox.Show($"Move '{executeUCITxtBox.Text}' is Invalid.");
                        }
                        else
                        {
                            processedSuccessfully = true;
                        }
                    }
                    else
                    {
                        if (!((Game)gamesListBox.Items[gamesListBox.SelectedIndex]).uciParser.executeUCIOperation(executeUCITxtBox.Text))
                        {
                            MessageBox.Show($"UCI Command '{executeUCITxtBox.Text}' is Invalid.");
                        }
                        else
                        {
                            processedSuccessfully = true;
                        }
                    }

                }
                catch
                {
                    MessageBox.Show("Invalid Game Selected");
                }
            }
            else
            {
                MessageBox.Show("Select a Game First.");
            }

            if (processedSuccessfully)
            {
                executeUCITxtBox.Text = "";
            }
        }

        private void executeUCITxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                executeUCIBtn.PerformClick();
            }
        }
    }


}
