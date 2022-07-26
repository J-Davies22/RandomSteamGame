using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Management;
using System.Diagnostics;

namespace RandomSteamGame
{
    public partial class Form1 : Form
    {
        Random rng;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rng = new Random();
        }

        private void ChooseRandomGame()
        {

            string chosen = GameLoader.gameDirectories.ElementAt(rng.Next(0, GameLoader.gameDirectories.Count));
            Process.Start(chosen);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseRandomGame();
        }
    }
}