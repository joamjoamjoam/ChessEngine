﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessEngine
{
    public partial class Form1 : Form
    {
        public Account currentAccount = null;
        private delegate void SafeCallDelegate();
        private delegate void ClockUpdateDelegate(Game sender, ChessmanColor color, int time);
        public bool enableLegends = false;


        MyCheckBox[,] chessBoardList = new MyCheckBox[8, 8]; // Match BoardSpace model
        MyCheckBox[,] opBoardList = new MyCheckBox[8, 8]; // Match BoardSpace model

        public Form1()
        {
            InitializeComponent();

            authTokenTxtBox.Text = "TzURxC7XfDjsGc5n";
            Text = "LiChess Chess Engine";

            oppClockNameLbl.Text = "";
            oppClockTimeLbl.Text = "";
            oppBoardNameLbl.Text = "";

            playerClockNameLbl.Text = "";
            playerClockTimeLbl.Text = "";

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

                if (selectedSpaces.Count == 2)
                {
                    String move = "";
                    MyCheckBox fromSpace = null;
                    MyCheckBox toSpace = null;
                    if (DateTime.Compare(selectedSpaces[0].lastTimeSelected , selectedSpaces[1].lastTimeSelected) < 0)
                    {
                        fromSpace = selectedSpaces[0];
                        toSpace = selectedSpaces[1];
                    }
                    else
                    {
                        fromSpace = selectedSpaces[1];
                        toSpace = selectedSpaces[0];
                    }

                    if (toSpace.BackColor == MyCheckBox.chessBoardValidMoveColor)
                    {
                        move = $"{fromSpace.Name.ToLower()}{toSpace.Name.ToLower()}";

                        if (!((Game)gamesListBox.SelectedItem).makeMove(move))
                        {
                            //toSpace.Checked = false;

                            if (toSpace.getLinkedBoardSpace().piece != null && toSpace.getLinkedBoardSpace().piece.color == toSpace.game.color)
                            {
                                // Select this piece instead
                                fromSpace.Checked = false;
                            }
                            else
                            {
                                toSpace.Checked = false;
                            }

                        }
                        else
                        {
                            toSpace.Checked = false;
                            fromSpace.Checked = false;
                        }
                    }
                    else
                    {
                        if (toSpace.getLinkedBoardSpace().piece != null && toSpace.getLinkedBoardSpace().piece.color == toSpace.game.color)
                        {
                            // Select this piece instead
                            resetCheckBoxDefaultColors(visualOnly: true);
                            fromSpace.Checked = false;
                            toSpace.BackColor = MyCheckBox.chessBoardSelectedColor;
                            toSpace.Checked = true;
                            updateValidSquaresForSelectedCB(toSpace);
                        }
                        else
                        {
                            //MessageBox.Show($"Move '{move}' is Invalid.");
                            toSpace.Checked = false;
                        }
                    }
                }
                else if (selectedSpaces.Count == 1)
                {
                    updateValidSquaresForSelectedCB(cb);
                }
                else
                {
                    // Non selected Reset Colors
                    resetCheckBoxDefaultColors(visualOnly: true);
                }
            }
        }

        private void updateValidSquaresForSelectedCB(MyCheckBox cb)
        {
            if (cb.Checked)
            {
                //cb.Image = ResizeImage(Properties.Resources.BlackKnight,  cb.Size.Width/2, cb.Size.Height/2);
                cb.BackColor = MyCheckBox.chessBoardSelectedColor;
            }
            else
            {
                cb.BackColor = cb.checkerBoardColor;
            }
            //Disable All Invalid moves
            Game currGame = ((Game)gamesListBox.SelectedItem);
            Board board = currGame.gameBoard;
            BoardSpace boardPos = cb.getLinkedBoardSpace();

            if (boardPos.piece != null && boardPos.piece.color == currGame.color)
            {
                List<Move> validMoves = boardPos.piece.getAvailableMoves(board);

                foreach (Move mv in validMoves)
                {
                    // Highlight valid Moves
                    bool found = false;
                    for (int row = 1; row <= 8; row++)
                    {
                        for (Char col = 'A'; col <= 'H'; col++)
                        {
                            MyCheckBox itCB = chessBoardList[row - 1, col - 'A'];
                            if (itCB.getLinkedBoardSpace().position.Item2 == mv.toSpace.position.Item2 && itCB.getLinkedBoardSpace().position.Item1 == mv.toSpace.position.Item1)
                            {
                                itCB.BackColor = MyCheckBox.chessBoardValidMoveColor;
                                found = true;
                                break; ;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                    }

                }

            }
        }

        private void opBoardSqaureCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                cb.Checked = false;
            }
        }

        private void opBoardSqaureGotFocus(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            this.Focus();
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

        private void clockStateUpdated(Game sender, ChessmanColor color, int time)
        {
            if (this.InvokeRequired)
            {
                ClockUpdateDelegate d = new ClockUpdateDelegate(clockStateUpdated);
                this.Invoke(d, new object[] { sender, color, time});
            }
            else
            {
                TimeSpan span = TimeSpan.FromMilliseconds(time);

                String timeStr = $"{((span.Hours != 0) ? $"{span.Hours.ToString("D2")}:" : "")}{((span.Minutes != 0 || span.Hours != 0 ) ? $"{(span.Minutes.ToString("D2"))}:" : "")}{((span.Seconds != 0 || (span.Minutes != 0 || span.Hours != 0)) ? $"{span.Seconds.ToString("D2")}" : "")}";

                if (span.TotalMilliseconds == 0)
                {
                    timeStr = "00:00:00";
                }

                if (color == ChessmanColor.white)
                {
                    if (sender.color == ChessmanColor.white)
                    {
                        playerClockTimeLbl.Text = timeStr;
                    }
                    else
                    {
                        oppClockTimeLbl.Text = timeStr;
                    }
                }
                else
                {
                    if (sender.color == ChessmanColor.black)
                    {
                        playerClockTimeLbl.Text = timeStr;
                    }
                    else
                    {
                        oppClockTimeLbl.Text = timeStr;
                    }
                }
            }
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
                currGame.clockStateUpdated += clockStateUpdated;
                currGame.BoardUpdated += updateGUIAfterBoardChange;
                currGame.GameOver += gameHasEnded;
                currGame.startGameStream();
                Console.WriteLine(currGame.getBoard().printBoard(currGame.color));

                Console.WriteLine(Environment.NewLine + ((Game)b.Items[b.SelectedIndex]).getBoard().printBoard((currGame.color == ChessmanColor.white) ? ChessmanColor.black : ChessmanColor.white) + Environment.NewLine);

                // Set Opponent Board and Clock Labels
                oppBoardNameLbl.Text = $"{currGame.opponentName}'s Board";
                oppClockNameLbl.Text = $"{currGame.opponentName}'s Clock";
                playerClockNameLbl.Text = $"{currentAccount.username}'s Clock";

                oppClockTimeLbl.Location = new Point(playerClockTimeLbl.Location.X,oppClockTimeLbl.Location.Y);

                oppClockTimeLbl.Width = playerClockTimeLbl.Width = Math.Max(oppClockNameLbl.Width, playerClockNameLbl.Width);

                int yPos = 0;
                int xPos = 0;
                int cbXDim = 100;
                int cbYDim = 100;
                int edgeBuffer = 15;
                Rectangle screenRectangle = RectangleToScreen(ClientRectangle);

                int titleHeight = screenRectangle.Top - Top;

                turnLbl.AutoSize = false;
                turnLbl.Size = new Size((8 * cbXDim), turnLbl.Size.Height);

                int initialXPos = oppClockNameLbl.Location.X + Math.Max(oppClockNameLbl.Width, playerClockNameLbl.Width) + edgeBuffer;

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

                            MyCheckBox cb = new MyCheckBox(tmp, effectiveRow, currGame);
                            cb.GotFocus += opBoardSqaureGotFocus;
                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.Click += boardSqaureCheckChanged;
                            cb.Name = $"{tmp}{effectiveRow}";

                            bool legendPrinted = false;
                            if (enableLegends)
                            {
                                if (col == 7 && effectiveRow == 8) // Use As Legend Corner
                                {
                                    // Set Row Legends
                                    cb.Text = $"{tmp.ToString().ToUpper()}{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                                else if (effectiveRow == 8) // Use As Legend
                                {
                                    // Set Column Legends
                                    cb.Text = tmp.ToString().ToUpper();
                                    cb.TextAlign = ContentAlignment.BottomCenter;
                                    legendPrinted = true;
                                }
                                else if (col == 7) // Use As Legend
                                {
                                    // Set Row Legends
                                    cb.Text = $"{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                            }
                            if(!legendPrinted)
                            {
                                cb.Text = cb.Name;
                                cb.TextAlign = ContentAlignment.BottomRight;
                                cb.Font = new Font(cb.Font.FontFamily, 8);
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

                            MyCheckBox cb = new MyCheckBox(tmp, row+1, currGame);
                            cb.GotFocus += opBoardSqaureGotFocus;
                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.Click += boardSqaureCheckChanged;
                            cb.Name = $"{tmp}{effectiveRow}";

                            bool legendPrinted = false;
                            if (enableLegends)
                            {
                                if (col == 0 && effectiveRow == 1) // Use As Legend Corner
                                {
                                    // Set Row Legends
                                    cb.Text = $"{tmp.ToString().ToUpper()}{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                                else if (effectiveRow == 1) // Use As Legend
                                {
                                    // Set Column Legends
                                    cb.Text = tmp.ToString().ToUpper();
                                    cb.TextAlign = ContentAlignment.BottomCenter;
                                    legendPrinted = true;
                                }
                                else if (col == 0) // Use As Legend
                                {
                                    // Set Row Legends
                                    cb.Text = $"{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                            }
                            if (!legendPrinted)
                            {
                                cb.Text = cb.Name;
                                cb.TextAlign = ContentAlignment.BottomRight;
                                cb.Font = new Font(cb.Font.FontFamily, 8);
                            }

                            cb.Size = new Size(cbXDim, cbYDim);
                            cb.Location = new Point(xPos, yPos);

                            Controls.Add(cb);
                            chessBoardList[row, col] = cb;
                        }
                    }
                }



                // Setup Opponents Board

                yPos = 0;
                xPos = 0;
                edgeBuffer = 15;

                initialXPos = edgeBuffer;

                initialYPos = oppBoardNameLbl.Height + oppBoardNameLbl.Location.Y + edgeBuffer;

                cbXDim = cbYDim = Math.Min((Size.Height - initialYPos - edgeBuffer)/8, (oppClockNameLbl.Location.X + (Math.Max(oppClockNameLbl.Width, playerClockNameLbl.Width) - initialXPos - 2*edgeBuffer)) / 8);

                oppBoardNameLbl.Width = 8 * cbXDim;



                if (!(currGame.color == ChessmanColor.black))
                {

                    for (int row = 7; row >= 0; row--)
                    {

                        // Initial Cb positon = 333x, 441y
                        int effectiveRow = 8 - row;
                        yPos = initialYPos + (cbYDim * (effectiveRow - 1));
                        for (int col = 7; col >= 0; col--)
                        {
                            xPos = initialXPos + (col * cbXDim);
                            Char tmp = (Char)('H' - col);

                            //boardState[row, col] = new BoardSpace(tmp, effectiveRow);

                            MyCheckBox cb = new MyCheckBox(tmp, effectiveRow, currGame);

                            cb.GotFocus += opBoardSqaureGotFocus;
                            cb.CheckedChanged += opBoardSqaureCheckedChanged;
                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.Name = $"{tmp}{effectiveRow}";

                            bool legendPrinted = false;
                            if (enableLegends)
                            {
                                if (col == 7 && effectiveRow == 8) // Use As Legend Corner
                                {
                                    // Set Row Legends
                                    cb.Text = $"{tmp.ToString().ToUpper()}{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                                else if (effectiveRow == 8) // Use As Legend
                                {
                                    // Set Column Legends
                                    cb.Text = tmp.ToString().ToUpper();
                                    cb.TextAlign = ContentAlignment.BottomCenter;
                                    legendPrinted = true;
                                }
                                else if (col == 7) // Use As Legend
                                {
                                    // Set Row Legends
                                    cb.Text = $"{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                            }
                            if (!legendPrinted)
                            {
                                cb.Text = cb.Name;
                                cb.TextAlign = ContentAlignment.BottomRight;
                                cb.Font = new Font(cb.Font.FontFamily, 6);
                            }

                            cb.Size = new Size(cbXDim, cbYDim);
                            cb.Location = new Point(xPos, yPos);

                            Controls.Add(cb);
                            opBoardList[row, col] = cb;


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

                            MyCheckBox cb = new MyCheckBox(tmp, (row+1), currGame);
                            cb.GotFocus += opBoardSqaureGotFocus;
                            cb.CheckedChanged += opBoardSqaureCheckedChanged;
                            cb.AutoSize = false;
                            cb.Appearance = Appearance.Button;
                            cb.ImageAlign = ContentAlignment.MiddleCenter;
                            cb.Name = $"{tmp}{effectiveRow}";

                            bool legendPrinted = false;
                            if (enableLegends)
                            {
                                if (col == 0 && effectiveRow == 1) // Use As Legend Corner
                                {
                                    // Set Row Legends
                                    cb.Text = $"{tmp.ToString().ToUpper()}{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                                else if (effectiveRow == 1) // Use As Legend
                                {
                                    // Set Column Legends
                                    cb.Text = tmp.ToString().ToUpper();
                                    cb.TextAlign = ContentAlignment.BottomCenter;
                                    legendPrinted = true;
                                }
                                else if (col == 0) // Use As Legend
                                {
                                    // Set Row Legends
                                    cb.Text = $"{effectiveRow}";
                                    cb.TextAlign = ContentAlignment.BottomRight;
                                    legendPrinted = true;
                                }
                            }
                            if (!legendPrinted)
                            {
                                cb.Text = cb.Name;
                                cb.TextAlign = ContentAlignment.BottomRight;
                                cb.Font = new Font(cb.Font.FontFamily, 6);
                            }

                            cb.Size = new Size(cbXDim, cbYDim);
                            cb.Location = new Point(xPos, yPos);

                            Controls.Add(cb);
                            opBoardList[row, col] = cb;

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
                if (gamesListBox.SelectedIndex >= 0)
                {
                    Game currGame = ((Game)gamesListBox.Items[gamesListBox.SelectedIndex]);

                    Console.WriteLine(currGame.getBoard().printBoard(currGame.color));
                    Console.WriteLine(Environment.NewLine + ((Game)gamesListBox.Items[gamesListBox.SelectedIndex]).getBoard().printBoard((currGame.color == ChessmanColor.white) ? ChessmanColor.black : ChessmanColor.white) + Environment.NewLine);

                    // Update ChessBoard GUI

                    Board board = ((Game)gamesListBox.SelectedItem).gameBoard;
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

                    if (!(((Game)gamesListBox.SelectedItem).color == ChessmanColor.white))
                    {
                        for (int row = 0; row < 8; row++)
                        {
                            for (int col = 0; col < 8; col++)
                            {
                                if (opBoardList[row, col] != null)
                                {
                                    if (currGame.gameBoard.getBoard()[row, col].piece != null)
                                    {
                                        opBoardList[row, col].Image = resizeImage(currGame.gameBoard.getBoard()[row, col].piece.getImage(), opBoardList[row, col].Size.Width / 2, opBoardList[row, col].Size.Height / 2);
                                    }
                                    else
                                    {
                                        opBoardList[row, col].Image = null;
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
                                if (opBoardList[row, col] != null)
                                {
                                    if (currGame.gameBoard.getBoard()[row, col].piece != null)
                                    {
                                        opBoardList[row, 7 - col].Image = resizeImage(currGame.gameBoard.getBoard()[row, col].piece.getImage(), opBoardList[row, col].Size.Width / 2, opBoardList[row, col].Size.Height / 2);
                                    }
                                    else
                                    {
                                        opBoardList[row, 7 - col].Image = null;
                                    }

                                }
                            }
                        }
                    }

                    // Update Turn Label
                    turnLbl.Text = $"{((currGame.color == currGame.playerTurn) ? $"{currentAccount.username}'s" : $"{currGame.opponentName}'s")} ({CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currGame.playerTurn.ToString().ToLower())}) Turn";

                    // Select last Move
                    if (currGame.lastMove != null && currGame.color == currGame.playerTurn)
                    {
                        if (false) // castles
                        {

                        }
                        else
                        {
                            MyCheckBox check = getCheckBoxForChessPosition(currGame.lastMove.toSpace.position.Item1, currGame.lastMove.toSpace.position.Item2, chessBoardList, currGame);
                            if (check != null)
                            {
                                check.checkerBoardColor = MyCheckBox.chessBoardLastMoveColor;
                                check.BackColor = check.checkerBoardColor;
                            }

                            check = getCheckBoxForChessPosition(currGame.lastMove.fromSpace.position.Item1, currGame.lastMove.fromSpace.position.Item2, chessBoardList, currGame);
                            if (check != null)
                            {
                                check.checkerBoardColor = MyCheckBox.chessBoardLastMoveColor;
                                check.BackColor = check.checkerBoardColor;
                            }
                        }
                    }
                    else
                    {
                        resetCheckBoxDefaultColors();
                    }

                    
                    if (currGame.color == currGame.playerTurn)
                    {
                        Thread.Sleep(2000);
                        Move bestMove = currGame.aiContext.getBestMove();
                        if (bestMove != null)
                        {
                            Debug.WriteLine($"Making Move: {bestMove.ToString()}");
                            currGame.makeMove(bestMove);
                        }
                    }
                }
            }



        }

        private void resetCheckBoxDefaultColors(bool visualOnly = false)
        {
            for (int row = 7; row >= 0; row--)
            {
                for (int col = 7; col >= 0; col--)
                {
                    if (opBoardList[row, col] != null)
                    {
                        if (!visualOnly)
                        {
                            opBoardList[row, col].checkerBoardColor = opBoardList[row, col].baseFieldColor;
                        }
                        opBoardList[row, col].BackColor = opBoardList[row, col].checkerBoardColor;
                    }
                    if (chessBoardList[row, col] != null)
                    {
                        if (!visualOnly)
                        {
                            chessBoardList[row, col].checkerBoardColor = opBoardList[row, col].baseFieldColor;
                        }
                        chessBoardList[row, col].BackColor = chessBoardList[row, col].checkerBoardColor;
                    }

                }
            }
        }

        private void gameHasEnded(Game sender, ChessmanColor winner, GameStatus res)
        {
            MessageBox.Show($"Game Over\nResult: {((winner == ChessmanColor.none) ? "Draw" : $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(winner.ToString())} Wins!")}\nReason: {res.ToString()}", "Game Has Ended");
        }

        public MyCheckBox getCheckBoxForChessPosition(char col, int row, MyCheckBox[,] board, Game currGame)
        {
            MyCheckBox rv = null;
            try
            {
                if (col >= 'A' && col <= 'H' && row >= 1 && row <= 8)
                {
                    rv = board[MyCheckBox.getIndexForBoardRow(row, currGame.color), MyCheckBox.getIndexForBoardColumn(col, currGame.color)];
                }
            }
            catch
            {

            }

            return rv;
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
        public static Color chessBoardDarkColor = Color.FromArgb(255, 0, 84, 229);
        public static Color chessBoardLightColor = Color.Gray;
        public static Color chessBoardLastMoveColor = Color.FromArgb(255, Color.MediumAquamarine);
        public static Color chessBoardValidMoveColor = Color.FromArgb(180, Color.LimeGreen);
        public static Color chessBoardSelectedColor = Color.Yellow;

        public DateTime lastTimeSelected = DateTime.Now;
        public Color baseFieldColor = chessBoardLightColor;
        public Color checkerBoardColor = chessBoardLightColor;

        public Char column = 'A';
        public int row = 1;
        public Game game = null;

        public MyCheckBox(Char column, int row, Game game)
        {
            checkerBoardColor = chessBoardLightColor;
            this.column = column;
            this.row = row;
            switch (column)
            {
                case 'A':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
                case 'B':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
                case 'C':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
                case 'D':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
                case 'E':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
                case 'F':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
                case 'G':
                    if (row == 1 || row == 3 || row == 5 || row == 7)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
                case 'H':
                    if (row == 2 || row == 4 || row == 6 || row == 8)
                    {
                        checkerBoardColor = baseFieldColor = chessBoardDarkColor;
                    }
                    break;
            }
            BackColor = checkerBoardColor;

            this.game = game;
        }

        public BoardSpace getLinkedBoardSpace() {
           return (game.color == ChessmanColor.white) ? game.gameBoard.getSpace(column, (8-row + 1)) : game.gameBoard.getSpace(column, row);
        }

        public static int getIndexForBoardRow(int row, ChessmanColor color)
        {
            return (color == ChessmanColor.white || true) ? 8 - row : row;
        }

        public static int getIndexForBoardColumn(Char col, ChessmanColor color)
        {
            return (color == ChessmanColor.white) ? col - 'A' : 'H' - col;
        }
    }


}
