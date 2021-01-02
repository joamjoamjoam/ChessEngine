using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
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



        MyCheckBox[,] chessBoardList = new MyCheckBox[8, 8]; // Match BoardSpace model

        MyCheckBox[,] opBoardList = new MyCheckBox[8, 8]; // Match BoardSpace model

        public Form1()
        {
            InitializeComponent();

            authTokenTxtBox.Text = "TzURxC7XfDjsGc5n";


        }

        private void boardSqaureCheckChanged(object sender, EventArgs e)
        {
            MyCheckBox cb = (MyCheckBox)sender;

            List<MyCheckBox> selectedSpaces = getSelectedSquareInChessBoard();
            cb.lastTimeSelected = DateTime.Now;

            if (selectedSpaces.Count > 2)
            {
                if (!cb.Checked)
                {
                    cb.Checked = false;
                }
            }
            else
            {
                if (cb.Checked)
                {
                    //cb.Image = ResizeImage(Properties.Resources.BlackKnight,  cb.Size.Width/2, cb.Size.Height/2);
                    cb.BackColor = Color.Yellow;
                }
                else
                {
                    cb.BackColor = cb.checkerBoardColor;
                }

                if (selectedSpaces.Count == 2)
                {


                    String move = "";
                    if (DateTime.Compare(selectedSpaces[0].lastTimeSelected , selectedSpaces[1].lastTimeSelected) < 0)
                    {
                        move = $"{selectedSpaces[0].Name.ToLower()}{selectedSpaces[1].Name.ToLower()}";
                    }
                    else
                    {
                        move = $"{selectedSpaces[1].Name.ToLower()}{selectedSpaces[0].Name.ToLower()}";
                    }

                    if (!((Game)gamesListBox.SelectedItem).makeMove(move))
                    {
                        MessageBox.Show($"Move '{move}' is Invalid.");
                    }

                    // Make Move and Clear boxes
                    foreach (MyCheckBox c in selectedSpaces)
                    {
                        c.Checked = false;
                    }

                }
                else if (selectedSpaces.Count == 1)
                {
                    // Disable All Invalid moves
                }
            }
        }

        private List<MyCheckBox> getSelectedSquareInChessBoard()
        {
            List<MyCheckBox> rv = new List<MyCheckBox>();
            Board board = ((Game)gamesListBox.SelectedItem).gameBoard;

            if (gamesListBox.SelectedIndex >= 0)
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (chessBoardList[row, col] != null && chessBoardList[row, col].Checked)
                        {
                            rv.Add(chessBoardList[row, col]);
                        }
                    }
                }
            }

            return rv;
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

        public static Bitmap resizeImage(Image image, int width, int height)
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

        private void gamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox b = (ListBox)sender;
            // Set Browser

            foreach (Control item in Controls.OfType<MyCheckBox>())
            {
                Controls.Remove(item);
                Array.Clear(chessBoardList, 0, chessBoardList.Length);
                Array.Clear(opBoardList, 0, opBoardList.Length);
            }

            if (b.SelectedIndex != -1)
            {

                Game currGame = ((Game)b.Items[b.SelectedIndex]);
                currGame.BoardUpdated += updateGUIAfterBoardChange;
                currGame.startGameStream();
                boardTextBox.Text = currGame.getBoard().printBoard(currGame.color);

                boardTextBox.Text += Environment.NewLine + ((Game)b.Items[b.SelectedIndex]).getBoard().printBoard((currGame.color == ChessmanColor.white) ? ChessmanColor.black : ChessmanColor.white) + Environment.NewLine;

                int yPos = 0;
                int xPos = 0;
                int cbXDim = 100;
                int cbYDim = 100;
                int edgeBuffer = 15;
                Rectangle screenRectangle = RectangleToScreen(ClientRectangle);

                int titleHeight = screenRectangle.Top - Top;

                turnLbl.AutoSize = false;
                turnLbl.Size = new Size((8 * cbXDim), turnLbl.Size.Height);
                turnLbl.BackColor = Color.Red;

                int initialXPos = boardTextBox.Location.X + boardTextBox.Width + edgeBuffer;

                turnLbl.Location = new Point(initialXPos, turnLbl.Location.Y);

                
                int initialYPos = turnLbl.Height + turnLbl.Location.Y + edgeBuffer;


                Size = new Size(initialXPos + (8 * cbXDim) + 2*edgeBuffer, initialYPos + (8 * cbYDim) + edgeBuffer + titleHeight);

                

                if (currGame.color == ChessmanColor.black)
                {

                    for (int row = 7; row >=0 ; row--)
                    {

                        // Initial Cb positon = 333x, 441y
                        int effectiveRow = 8 - row;
                        yPos = initialYPos + (cbYDim * (effectiveRow - 1));
                        for (int col = 7; col >= 0; col--)
                        {
                            xPos = initialXPos + (col * cbXDim);
                            Char tmp = (Char)('H' - col);
                            
                            //boardState[row, col] = new BoardSpace(tmp, effectiveRow);

                            MyCheckBox cb = new MyCheckBox(tmp, effectiveRow);

                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.CheckedChanged += boardSqaureCheckChanged;
                            cb.Name = $"{tmp}{effectiveRow}";

                            if (col == 7 && effectiveRow == 8) // Use As Legend Corner
                            {
                                // Set Row Legends
                                cb.Text = $"           {tmp.ToString().ToUpper()}           {effectiveRow}";
                                cb.TextAlign = ContentAlignment.BottomLeft;
                            }
                            else if (effectiveRow == 8) // Use As Legend
                            {
                                // Set Column Legends
                                cb.Text = tmp.ToString().ToUpper();
                                cb.TextAlign = ContentAlignment.BottomCenter;
                            }
                            else if (col == 7) // Use As Legend
                            {
                                // Set Row Legends
                                cb.Text = $"{effectiveRow}";
                                cb.TextAlign = ContentAlignment.BottomRight;
                            }
                            else
                            {
                                cb.Text = cb.Name;
                            }

                            cb.Size = new Size(cbXDim, cbYDim);
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
                        // Initial Cb positon = 333x, 441y
                        yPos = initialYPos + (cbYDim * row);
                        for (int col = 0; col < 8; col++)
                        {
                            xPos = initialXPos + (col * cbXDim);
                            Char tmp = (Char)('A' + col);
                            int effectiveRow = 8 - row;
                            //boardState[row, col] = new BoardSpace(tmp, effectiveRow);

                            MyCheckBox cb = new MyCheckBox(tmp, row);

                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.CheckedChanged += boardSqaureCheckChanged;
                            cb.Name = $"{tmp}{effectiveRow}";

                            if (col == 0 && effectiveRow == 1) // Use As Legend Corner
                            {
                                // Set Row Legends
                                cb.Text = $"{effectiveRow}           {tmp}";
                                cb.TextAlign = ContentAlignment.BottomLeft;
                            }
                            else if (effectiveRow == 1) // Use As Legend
                            {
                                // Set Column Legends
                                cb.Text = tmp.ToString().ToUpper();
                                cb.TextAlign = ContentAlignment.BottomCenter;
                            }
                            else if (col == 0) // Use As Legend
                            {
                                // Set Row Legends
                                cb.Text = $"{effectiveRow}";
                                cb.TextAlign = ContentAlignment.BottomLeft;
                            }
                            else
                            {
                                cb.Text = cb.Name;
                            }

                            cb.Size = new Size(cbXDim, cbYDim);
                            cb.Location = new Point(xPos, yPos);

                            Controls.Add(cb);
                            chessBoardList[row, col] = cb;


                        }
                    }
                }
                updateGUIAfterBoardChange();
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
                    boardTextBox.Text += Environment.NewLine + ((Game)gamesListBox.Items[gamesListBox.SelectedIndex]).getBoard().printBoard((currGame.color == ChessmanColor.white) ? ChessmanColor.black : ChessmanColor.white) + Environment.NewLine;

                    // Update ChessBoard GUI

                    Board board = ((Game)gamesListBox.SelectedItem).gameBoard;

                    if (gamesListBox.SelectedIndex >= 0)
                    {
                        if (((Game)gamesListBox.SelectedItem).color == ChessmanColor.white)
                        {
                            for (int row = 0; row < 8; row++)
                            {
                                for (int col = 0; col < 8; col++)
                                {
                                    if (chessBoardList[row, col] != null)
                                    {
                                        if (currGame.gameBoard.getBoard()[row, col].piece != null)
                                        {
                                            chessBoardList[row, col].Image = resizeImage(currGame.gameBoard.getBoard()[row, col].piece.getImage(), chessBoardList[row, col].Size.Width / 2, chessBoardList[row, col].Size.Height / 2);
                                        }
                                        else
                                        {
                                            chessBoardList[row, col].Image = null;
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int row = 7; row >= 0; row--)
                            {
                                for (int col = 7; col >= 0; col--)
                                {
                                    if (chessBoardList[row, col] != null)
                                    {
                                        if (currGame.gameBoard.getBoard()[row, col].piece != null)
                                        {
                                            chessBoardList[row, 7-col].Image = resizeImage(currGame.gameBoard.getBoard()[row, col].piece.getImage(), chessBoardList[row, col].Size.Width / 2, chessBoardList[row, col].Size.Height / 2);
                                        }
                                        else
                                        {
                                            chessBoardList[row, 7-col].Image = null;
                                        }

                                    }
                                }
                            }
                        }

                        

                        // Update Turn Label
                        turnLbl.Text = $"{((currGame.color == currGame.playerTurn) ? $"{currentAccount.username}'s" : $"{currGame.opponentName}'s")} ({CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currGame.playerTurn.ToString().ToLower())}) Turn";

                    }

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

    public class MyCheckBox : CheckBox
    {
        Color chessBoardDarkColor = Color.SteelBlue;
        Color chessBoardLightColor = Color.LightSlateGray;

        public DateTime lastTimeSelected = DateTime.Now;
        public Color checkerBoardColor;

        public MyCheckBox(Char column, int row)
        {
            checkerBoardColor = chessBoardLightColor;
            
            switch (column)
            {
                case 'A':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
                case 'B':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
                case 'C':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
                case 'D':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
                case 'E':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
                case 'F':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
                case 'G':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
                case 'H':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = chessBoardDarkColor;
                    }
                    break;
            }
            BackColor = checkerBoardColor;
        }
    }


}
