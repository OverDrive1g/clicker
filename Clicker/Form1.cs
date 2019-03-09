using Clicker.core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicker
{
    public partial class Form1 : Form
    {
        private Random rnd;
        private double score = 0;
        private double clickPower = 1;
        private double powerCrit;
        private double priceCrit;
        private bool isBoost = false;

        private Image currImg;
        private Image critImg;

        BindingList<BaseUpgrade> upgradeList;
        BindingList<Statistic> statisticsList;

        public Form1()
        {
            InitializeComponent();
            rnd = new Random();

            powerCrit = 5;
            priceCrit = 10;
            
            upgradeList = new BindingList<BaseUpgrade>()
            {
                new BaseUpgrade("Test1", 10, 1.07, 0,1),
                new BaseUpgrade("Test2", 100, 1.08, 0,5),
                new BaseUpgrade("Test3", 1000, 1.09, 0,10),
                new BaseUpgrade("Test4", 10000, 1.1, 0,50),
                new BaseUpgrade("Test5", 100000, 1.2, 0,100)
            };

            statisticsList = new BindingList<Statistic>()
            {
                new Statistic("Количество кликов вручную",0),
                new Statistic("Количество автокликов",0),
                new Statistic("Куплено улучшений",0),
                new Statistic("Количество турбобустов",0),
                new Statistic("Количество критов",0),
                new Statistic("Всего очков",0),
                new Statistic("Очки за криты",0)
            };
            dataGridView2.DataSource = statisticsList;

            var source = new BindingSource(upgradeList, null);

            dataGridView1.DataSource = source;

            currImg = global::Clicker.Properties.Resources._16;
            critImg = global::Clicker.Properties.Resources._17;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            statisticsList[0].Value++;
            doClick();
            dataGridView2.Refresh();
        }

        private void doClick()
        {
            
            var i = rnd.Next(10);
            var j = rnd.Next(100);

            if (j == 1 && !isBoost) { startBoost(); }

            var booster = 0;

            if (isBoost)
            {
                booster = 10;

            }
            else
            {
                booster = 1;
            }
            var inc = (i == 1 ? getClickStrange() * powerCrit : getClickStrange()) * booster;

            if (i == 1)
            {
                statisticsList[6].Value += inc;
                dataGridView2.Refresh();
            }

            score+=inc;

            statisticsList[5].Value += inc;
            dataGridView2.Refresh();

            if (i == 1)
            {
                this.pictureBox1.Image = critImg;
                statisticsList[4].Value++;
                dataGridView2.Refresh();
            }

            updateScore();
        }

        public double getClickStrange()
        {
            return clickPower;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureClickDown();

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureClickUp();
        }

        private void updateScore()
        {
            this.scoreLabel.Text = "Счет: " + score;
            label3.Text = "Сила клика: "+getClickStrange();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var item = upgradeList[e.RowIndex];
                if (score >= item.Price)
                {
                    score -= item.Price;
                    item.Count++;

                    updateScore();
                }
                statisticsList[2].Value++;
                dataGridView2.Refresh();
                dataGridView1.Refresh();

                var newPower = upgradeList.Sum(itm => itm.Power * itm.Count);
                clickPower = newPower + 1;
            }
            else if (e.ColumnIndex == 1)
            {
                var item = upgradeList[e.RowIndex];

                while (score >= item.Price)
                {
                    score -= item.Price;
                    item.Count++;
                    statisticsList[2].Value++;
                    dataGridView2.Refresh();
                }

                updateScore();

                dataGridView1.Refresh();

                var newPower = upgradeList.Sum(itm => itm.Power * itm.Count);
                clickPower = newPower + 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (score >= priceCrit)
            {
                score -= priceCrit;
                priceCrit *= 10;

                powerCrit *= 1.5;
            }
            label2.Text = "Сила крита: x" + powerCrit;
            button1.Text = "Цена - " + priceCrit;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                timer1.Start();
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureClickDown();
            pictureBox1.Update();
            statisticsList[1].Value++;
            doClick();
            dataGridView2.Refresh();
            System.Threading.Thread.Sleep(50);
            pictureClickUp();

        }

        private void pictureClickDown() {
            pictureBox1.Image = currImg;
            var curLocation = pictureBox1.Location;
            curLocation.X += 5;
            curLocation.Y += 5;

            pictureBox1.Location = curLocation;


            var curSize = pictureBox1.Size;
            curSize.Height -= 10;
            curSize.Width -= 10;

            pictureBox1.Size = curSize;
        }

        private void pictureClickUp() {
            var curLocation = pictureBox1.Location;
            curLocation.X -= 5;
            curLocation.Y -= 5;

            pictureBox1.Location = curLocation;


            var curSize = pictureBox1.Size;
            curSize.Height += 10;
            curSize.Width += 10;

            pictureBox1.Size = curSize;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            progressBar1.Value += 1;

            if (progressBar1.Value == 100)
            {
                timer2.Stop();
                stopBoost();
            }            
        }

        private void startBoost()
        {
            isBoost = true;
            currImg = global::Clicker.Properties.Resources._18;
            progressBar1.Value = 0;
            timer2.Start();
            this.BackColor = Color.LightPink;

            statisticsList[3].Value++;
            dataGridView2.Refresh();
        }

        private void stopBoost()
        {
            isBoost = false;
            currImg = global::Clicker.Properties.Resources._16;
            progressBar1.Value = 0;
            timer2.Stop();
            BackColor = System.Drawing.SystemColors.Control;

        }
    }
}
