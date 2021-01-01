using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public Form1()
        {
            InitializeComponent();

            authTokenTxtBox.Text = "TzURxC7XfDjsGc5n";
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
