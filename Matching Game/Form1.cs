using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matching_Game
{
    public partial class frmMatchGame : Form
    {
        public frmMatchGame()
        {
            InitializeComponent();
        }

        int[] photoIndex = new int[20];
        int photosRemaining;
        int[] score = new int[2];
        bool[] photoFound = new bool[20];
        int playerNumber, choiceNumber;
        int[] choice = new int[2];
        bool canClick = false;
        bool gameOver;
        PictureBox[] photoBox = new PictureBox[20];
        Image[] photo = new Image[10];
        Label[] lblScore = new Label[2];

        Random myRandom = new Random();
        bool smartComputer;
        int difficulty = 1;
        int boxSelected;

        int[] memory = new int[20];
        int[] matchFound = new int[2];

        System.Media.SoundPlayer matchSound;
        System.Media.SoundPlayer noMatchSound;
        
        System.Media.SoundPlayer gameOverSound;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmMatchGame_Load(object sender, EventArgs e)
        {
            // control array
            photoBox[0] = pictureBox1;
            photoBox[1] = pictureBox2;
            photoBox[2] = pictureBox3;
            photoBox[3] = pictureBox4;
            photoBox[4] = pictureBox5;
            photoBox[5] = pictureBox6;
            photoBox[6] = pictureBox7;
            photoBox[7] = pictureBox8;
            photoBox[8] = pictureBox9;
            photoBox[9] = pictureBox10;
            photoBox[10] = pictureBox11;
            photoBox[11] = pictureBox12;
            photoBox[12] = pictureBox13;
            photoBox[13] = pictureBox14;
            photoBox[14] = pictureBox15;
            photoBox[15] = pictureBox16;
            photoBox[16] = pictureBox17;
            photoBox[17] = pictureBox18;
            photoBox[18] = pictureBox19;
            photoBox[19] = pictureBox20;
            // image array
            for (int i = 0; i < 10; i++)
            {
                photo[i] = photoBox[i].Image;
            }
            // initialize boxes
            for (int i = 0; i < 20; i++)
            {
                photoBox[i].Image = picCover.Image;
            }
            lblScore[0] = lblScore1;
            lblScore[1] = lblScore2;
            grpPlayWho.Enabled = false;
            grpDifficulty.Enabled = false;

            matchSound = new
            System.Media.SoundPlayer(Application.StartupPath + "\\noMatchSound.wav");
            noMatchSound = new
            System.Media.SoundPlayer(Application.StartupPath + "\\beep.wav");
            gameOverSound = new
            System.Media.SoundPlayer(Application.StartupPath + "\\noMatchSound.wav");
        }

        private void rdoOnePlayer_CheckedChanged(object sender, EventArgs e)
        {
            grpPlayWho.Enabled = true;
            lblPlayer1.Text = "You";
            if (rdoPlayAlone.Checked)
            {
                lblPlayer2.Text = "Guesses";
                grpDifficulty.Enabled = false;
            }
            else
            {
                lblPlayer2.Text = "Computer";
                grpDifficulty.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rdoPlayAlone_CheckedChanged(object sender, EventArgs e)
        {
            grpDifficulty.Enabled = false;
            lblPlayer2.Text = "Guesses";
        }

        private void rdoPlayComputer_CheckedChanged(object sender, EventArgs e)
        {
            grpDifficulty.Enabled = true;
            lblPlayer2.Text = "Computer";
        }

        private void rdoTwoPlayers_CheckedChanged(object sender, EventArgs e)
        {
            grpPlayWho.Enabled = false;
            grpDifficulty.Enabled = false;
            lblPlayer1.Text = "Player 1";
            lblPlayer2.Text = "Player 2";
        }
        private int[] NRandomIntegers(int n)
        {
            // returns n randomly sorted integers
            int[] nIntegers = new int[n];
            int temp, s;
            Random sortRandom = new Random();
            // initialize array from 0 to n -1
            for (int i = 0; i < n; i++)
            {
                nIntegers[i] = i;
            }
            // i is number of items remaining in list
            for (int i = n; i >= 1; i--)
            {
                s = sortRandom.Next(i);
                temp = nIntegers[s];
                nIntegers[s] = nIntegers[i - 1];
                nIntegers[i - 1] = temp;
            }
            return (nIntegers);
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "Start Game")
            {
                btnStartStop.Text = "Stop Game";
                score[0] = 0;
                score[1] = 0;
                lblScore[0].Text = "0";
                lblScore[1].Text = "0";
                photosRemaining = 20;
                photoIndex = NRandomIntegers(20);
                for (int i = 0; i < 20; i++)
                {
                    if (photoIndex[i] > 9)
                    {
                        photoIndex[i] -= 10;
                    }
                    photoFound[i] = false;
                    photoBox[i].Image = picCover.Image;
                    photoBox[i].Visible = true;
                    memory[i] = 0;
                }
                playerNumber = 1;
                choiceNumber = 1;
                if (rdoTwoPlayers.Checked)
                {
                    lblMessage.Text = "Player 1, Pick a Box";
                }
                else
                {
                    lblMessage.Text = "Pick a Box";
                }
                grpNumberPlayers.Enabled = false;
                grpPlayWho.Enabled = false;
                grpDifficulty.Enabled = false;
                btnExit.Enabled = false;
                canClick = true;
                gameOver = false;
            }
            else
            {
                // stop game
                btnStartStop.Text = "Start Game";
                grpNumberPlayers.Enabled = false;
                if (rdoOnePlayer.Checked)
                {
                    grpPlayWho.Enabled = false;
                    if (rdoPlayComputer.Checked)
                    {
                        grpDifficulty.Enabled = true;
                    }
                }
                btnExit.Enabled = true;
                canClick = false;
                if (!gameOver)
                {
                    lblMessage.Text = "Game Stopped";
                }
                btnExit.Enabled = true;
                grpNumberPlayers.Enabled = true;
            }
        }

        private void PhotoBox_Click(object sender, EventArgs e)
        {
            // determine which box was clicked
            PictureBox clickedBox;
            int boxIndex;
            clickedBox = (PictureBox)sender;
            // number after PictureBox in name minus one is boxIndex
            boxIndex = Convert.ToInt32(clickedBox.Name.Substring(10, clickedBox.Name.Length - 10)) - 1;
            if (!canClick || photoFound[boxIndex])
                return;
            // one player/solitaire game
            if (rdoOnePlayer.Checked && rdoPlayAlone.Checked)
            {
                score[1]++;
                lblScore[1].Text = score[1].ToString();
            }
            // show image behind selected Photo box
            photoBox[boxIndex].Image = photo[photoIndex[boxIndex]];
            photoBox[boxIndex].Refresh();
            photoFound[boxIndex] = true;
            if (choiceNumber == 1)
            {
                choice[0] = boxIndex;
                choiceNumber = 2;
                memory[boxIndex] = photoIndex[boxIndex];
                if (rdoTwoPlayers.Checked)
                    lblMessage.Text = "Player " + playerNumber.ToString() + ", Pick Another Box";
                else
                {
                    // one player logic
                    if (playerNumber == 1)
                        lblMessage.Text = "Pick Another Box";
                    else
                    {
                        // play computer logic
                        lblMessage.Text = "Picking Another";
                        ComputerTurn();
                        return;
                    }
                }
            }
            else
            {
                choice[1] = boxIndex;
                choiceNumber = 1;
                memory[boxIndex] = photoIndex[boxIndex];
                if (photoIndex[choice[0]] == photoIndex[choice[1]])
                {
                    // a match
                    matchSound.PlaySync();
                    photoBox[choice[0]].Image = null;
                    photoBox[choice[1]].Image = null;
                    // clear memory so boxes are not checked again for match
                    memory[choice[0]] = 0;
                    memory[choice[1]] = 0;

                    score[playerNumber - 1]++;
                    lblScore[playerNumber - 1].Text = score[playerNumber - 1].ToString();
                    lblScore[playerNumber - 1].Refresh();
                    photosRemaining -= 2;
                    if (photosRemaining == 0)
                    {
                        gameOver = true;
                        gameOverSound.Play();
                        if (rdoTwoPlayers.Checked)
                        {
                            if (score[0] > score[1])
                                lblMessage.Text = "Player 1 Wins!";
                            else if (score[1] > score[0])
                                lblMessage.Text = "Player 2 Wins!";
                            else
                                lblMessage.Text = "It's a Tie!";
                        }
                        else
                        {
                            // one player logic
                            if (rdoPlayAlone.Checked)
                                lblMessage.Text = "All Matches Found!";
                            else
                            {
                                // play computer logic
                                if (score[0] > score[1])
                                {
                                    lblMessage.Text = "You Win!";
                                } 
                                else if (score[1] > score[0])
                                    lblMessage.Text = "Computer Wins!";
                                else
                                    lblMessage.Text = "It's a Tie!";
                            }
                        }
                        btnStartStop.PerformClick();
                        return;
                    }
                    // another turn
                    if (rdoTwoPlayers.Checked)
                        lblMessage.Text = "Player " + playerNumber.ToString() + ", Pick Again";
                    else
                    {
                        // one player logic
                        if (playerNumber == 1)
                            lblMessage.Text = "Pick a Box";
                        else
                        {
                            // play computer logic
                            lblMessage.Text = "Picking Again";
                            ComputerTurn();
                            return;
                        }
                    }
                }
                else
                {
                    // no match
                    noMatchSound.PlaySync();
                    photoFound[choice[0]] = false;
                    photoFound[choice[1]] = false;
                    photoBox[choice[0]].Image = picCover.Image;
                    photoBox[choice[0]].Refresh();
                    photoBox[choice[1]].Image = picCover.Image;
                    photoBox[choice[1]].Refresh();
                    // swap players
                    if (rdoTwoPlayers.Checked)
                    {
                        if (playerNumber == 1)
                        {
                            playerNumber = 2;
                        }
                        else
                        {
                            playerNumber = 1;
                        }
                        lblMessage.Text = "Player " + playerNumber.ToString() + ", Pick a Box";
                    }
                    else
                    {
                        // one player logic
                        if (rdoPlayComputer.Checked)
                        {
                            if (playerNumber == 1)
                                playerNumber = 2;
                            else
                                playerNumber = 1;
                        }

                        if (playerNumber == 1)
                            lblMessage.Text = "Pick a Box";
                        else
                        {
                            // play computer logic
                            lblMessage.Text = "Computer Picking";

                            choiceNumber = 1;
                            ComputerTurn();

                            return;
                        }
                    }
                }
            }
        }

        private void rdoEasiest_CheckedChanged(object sender, EventArgs e)
        {
            difficulty = 1;
        }
        private void rdoEasy_CheckedChanged(object sender, EventArgs e)
        {
            difficulty = 2;
        }
        private void rdoHard_CheckedChanged(object sender, EventArgs e)
        {
            difficulty = 3;
        }
        private void rdoHardest_CheckedChanged(object sender, EventArgs e)
        {
            difficulty = 4;
        }

        private int RandomChoice()
        {
            int count, n, rc;
            if (choiceNumber == 1)
                n = myRandom.Next(photosRemaining) + 1;
            else
                n = myRandom.Next(photosRemaining - 1) + 1;
            count = 0;
            for (rc = 0; rc < 20; rc++)
            {
                if (!photoFound[rc])
                    count++;
                if (count == n)
                    break;
            }
            return (rc);
        }
        private int SmartChoice()
        {
            int sc;
            if (choiceNumber == 1)
            {
                matchFound = CheckForMatch();
                if (matchFound[0] != 0 && matchFound[1] != 0)
                    sc = matchFound[0];
                else
                    sc = RandomChoice();
            }
            else
            {
                if (matchFound[0] != 0 && matchFound[1] != 0)
                    sc = matchFound[1];
                else
                {
                    matchFound = CheckForMatch();
                    if (matchFound[0] != 0 && matchFound[1] != 0)
                    {
                        if (matchFound[0] != choice[0])
                            sc = matchFound[0];
                        else
                            sc = matchFound[1];
                    }
                    else
                        sc = RandomChoice();
                }
            }
            return (sc);
        }

        private void timDelay_Tick(object sender, EventArgs e)
        {
            timDelay.Enabled = false;
            PhotoBox_Click(photoBox[boxSelected], null);
        }



        private void ComputerTurn()
        {
            int threshold = 0;
            if (choiceNumber == 1)
            {
                switch (difficulty)
                {
                    case 1:
                        threshold = 25;
                        break;
                    case 2:
                        threshold = 50;
                        break;
                    case 3:
                        threshold = 75;
                        break;
                    case 4:
                        threshold = 90;
                        break;
                }
                if (myRandom.Next(100) < threshold)
                    smartComputer = true;
                else
                    smartComputer = false;
            }
            if (smartComputer)
                boxSelected = SmartChoice();
            else
                boxSelected = RandomChoice();
            timDelay.Enabled = true;
        }

        private int[] CheckForMatch()
        {
            int[] matches = new int[2];
            matches[0] = 0;
            matches[1] = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (memory[i] != 0 && memory[i] == memory[j]
                    && i != j)
                    {
                        matches[0] = i;
                        matches[1] = j;
                    }
                }
            }
            return (matches);
        }
    }
}

// page 50