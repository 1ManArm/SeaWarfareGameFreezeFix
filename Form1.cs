using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            for (int i = 0; i < 10; i++)
                dataGridView1.Rows.Add(row);
            dataGridView1.ClearSelection();
        }
        model Model;
        string[] row = { "", "", "", "", "", "", "", "", "", "" };
        int x4 = 1,
            x3 = 2,
            x2 = 3,
            x1 = 4;

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
            if (Model.LastShot == ShotStatus.Wounded)
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
            while (Model.EnemyShips[x, y] != CoordStatus.None);
            textBox1.Text = s;
        }

        private void button104_Click(object sender, EventArgs e) //Перерисовать
        {
            /*var b = this.Controls.Find("b", true);*/ //Массив кнопок с именем b
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    string name = "b" + x.ToString() + y.ToString();
                    var b = this.Controls.Find(name, true);

                    if (b.Count() > 0)
                    {
                        var btn = b[0];
                        switch (Model.PlayerShips[x, y])
                        {
                            case CoordStatus.Ship: btn.Text = "x"; break;
                            case CoordStatus.None: btn.Text = ""; break;
                        }
                    }
                }
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    switch (Model.PlayerShips[x, y])
                    {
                        case CoordStatus.Ship:
                            dataGridView1[x, y].Value = "x"; break;
                        case CoordStatus.None:
                            dataGridView1[x, y].Value = ""; break;
                    }
                }
        }

        private void button103_Click(object sender, EventArgs e)
        {
            int cnt = dataGridView1.SelectedCells.Count;
            if (cnt > 0)
            {
                if (checkDelete.Checked)
                {
                    int a, b;
                    a = dataGridView1.SelectedCells[0].RowIndex;
                    b = dataGridView1.SelectedCells[0].ColumnIndex;
                    if (dataGridView1.Rows[a].Cells[b].Value.ToString() == "") return;
                }
                if (cnt == 1)
                    if (!checkDelete.Checked)
                    {
                        if (x1 == 0) return;
                        x1--;
                    }
                    else x1++;
                if (cnt == 2)
                    if (!checkDelete.Checked)
                    {
                        if (x2 == 0) return;
                        x2--;
                    }
                    else x2++;
                if (cnt == 3)
                    if (!checkDelete.Checked)
                    {
                        if (x3 == 0) return;
                        x3--;
                    }
                    else x3++;
                if (cnt == 4)
                    if (!checkDelete.Checked)
                    {
                        if (x4 == 0) return;
                        x4--;
                    }
                    else x4++;

                if (x1 < 0 || x2 < 0 || x3 < 0 || x4 < 0) return;
                if (dataGridView1.SelectedCells.Count > 1)
                {
                    for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                    {
                        int x = dataGridView1.SelectedCells[i].ColumnIndex;
                        int y = dataGridView1.SelectedCells[i].RowIndex;
                        CoordStatus coordStatus;
                        if (!checkDelete.Checked)
                            coordStatus = CoordStatus.Ship;
                        else coordStatus = CoordStatus.None;
                        Model.PlayerShips[x, y] = coordStatus;
                    }
                    dataGridView1.ClearSelection();
                }
                else
                {
                    Direction dir;
                    ShipType shipType = ShipType.x1;
                    if (checkVertical.Checked)
                    {
                        dir = Direction.Vertical;
                    }
                    else
                    {
                        dir = Direction.Horizontal;
                    }
                    if (radioButton1.Checked) shipType = ShipType.x1;
                    if (radioButton2.Checked) shipType = ShipType.x2;
                    if (radioButton3.Checked) shipType = ShipType.x3;
                    if (radioButton4.Checked) shipType = ShipType.x4;

                    Model.AddDelShip(textBox1.Text, shipType, dir, checkDelete.Checked);
                }
                button104_Click(sender, e);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int y = dataGridView1.SelectedCells[0].RowIndex;
            int x = dataGridView1.SelectedCells[0].ColumnIndex;
            textBox1.Text = x.ToString() + y.ToString();
        }

        private void checkDelete_Click(object sender, EventArgs e)
        {
            if (checkDelete.Checked)
            {
                button103.Text = "Удалить";
            }
            else
            {
                button103.Text = "Поставить";
            }
        }

        private void b00_Click(object sender, EventArgs e)
        {
            Form1_Load(sender,e);
            Button_Click(sender, e);
        }

        private void b01_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b02_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b03_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b04_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b05_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b06_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b07_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b08_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b09_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b10_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b11_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b12_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b13_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b14_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b15_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b16_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b17_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b18_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b19_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b20_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b21_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b22_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b23_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b24_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b25_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b26_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b27_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b28_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b29_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b30_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b31_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b32_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b33_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b34_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b35_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b36_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b37_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b38_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b39_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b40_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b41_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b42_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b43_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b44_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b45_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b46_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b47_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b48_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b49_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b50_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b51_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b52_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b53_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b54_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b55_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b56_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b57_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b58_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b59_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b60_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b61_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b62_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b63_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b64_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b65_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b66_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b67_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b68_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b69_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b70_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b71_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b72_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b73_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b74_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b75_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b76_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b77_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b78_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b79_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b80_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b81_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b82_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b83_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b84_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b85_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b86_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b87_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b88_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b89_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b90_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b91_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b92_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }

        private void b93_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b94_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b95_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b96_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b97_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b98_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void b99_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
            Button_Click(sender, e);
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int cnt = dataGridView1.SelectedCells.Count;
            textBox1.Text = cnt.ToString();
            if (cnt > 4)
            {
                MessageBox.Show("Превышено количество клеток!");
                int x = dataGridView1.SelectedCells[cnt - 1].ColumnIndex;
                int y = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows[y].Cells[x].Selected = false;
                dataGridView1.ClearSelection();
            }
            //if (cnt == 4) dataGridView1.SelectedCells.
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Настройка обработчиков для уже существующих кнопок
            for (int i = 0; i < 100; i++)
            {
                string buttonName = $"b{i / 10}{i % 10}"; // Генерация имен кнопок
                Button button = this.Controls.Find(buttonName, true).FirstOrDefault() as Button;

                if (button != null)
                {
                    button.Click += Button_Click;
                }
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string buttonName = button.Name; // Получаем имя кнопки
                string coordinates = buttonName.Substring(1); // Извлекаем координаты без буквы 'b'

                // Обновляем textBox1
                textBox1.Text = coordinates;
            }
        }
        

    }
}
