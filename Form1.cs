using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaWarfareGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Model = new model();
            Model.PlayerShips[0, 0] = CoordStatus.Ship;
            Model.PlayerShips[5, 2] = CoordStatus.Ship;
            Model.PlayerShips[5, 3] = CoordStatus.Ship;
            Model.PlayerShips[5, 4] = CoordStatus.Ship;
            Model.PlayerShips[7, 3] = CoordStatus.Ship;
        }
        model Model;
        private void button1_Click(object sender, EventArgs e)
        {
            Model.LastShot = Model.Shot(textBox1.Text);
            int x = int.Parse(textBox1.Text.Substring(0, 1));
            int y = int.Parse(textBox1.Text.Substring(1));
            switch (Model.LastShot)
            {
                case ShotStatus.Miss:
                    Model.EnemyShips[x, y] = CoordStatus.Shot; break;
                case ShotStatus.Wounded:
                    Model.EnemyShips[x, y] = CoordStatus.Hit; break;
                case ShotStatus.Kill:
                    Model.EnemyShips[x, y] = CoordStatus.Hit; break;
            }
            if(Model.LastShot == ShotStatus.Wounded)
            {
                Model.LastShotCoord = textBox1.Text;
                Model.WoundedStatus = true;
            }
            MessageBox.Show(Model.LastShot.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s;
            int x, y;
            do
            {
                s = Model.ShotGen();
                x = int.Parse(s.Substring(0, 1));
                y = int.Parse(s.Substring(1));
            }
            while (Model.EnemyShips[x, y] != CoordStatus.None) ;
            textBox1.Text = s;
        }
    }
}
