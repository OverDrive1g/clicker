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
        private readonly Random _rnd;
        private double _score;
        private double _clickPower = 1;
        private double _powerCrit;
        private double _priceCrit;
        private bool _isBoost;

        private Image _currImg;
        private readonly Image _critImg;

        readonly BindingList<BaseUpgrade> _upgradeList;
        readonly BindingList<Statistic> _statisticsList;

        public Form1()
        {
            InitializeComponent();
            _rnd = new Random();

            _powerCrit = 5;
            _priceCrit = 10;
            
            _upgradeList = new BindingList<BaseUpgrade>
            {
                new BaseUpgrade("Test1", 10, 1.07, 0,1),
                new BaseUpgrade("Test2", 100, 1.08, 0,5),
                new BaseUpgrade("Test3", 1000, 1.09, 0,10),
                new BaseUpgrade("Test4", 10000, 1.1, 0,50),
                new BaseUpgrade("Test5", 100000, 1.2, 0,100)
            };

            _statisticsList = new BindingList<Statistic>
            {
                new Statistic("Количество кликов вручную",0),
                new Statistic("Количество автокликов",0),
                new Statistic("Куплено улучшений",0),
                new Statistic("Количество турбобустов",0),
                new Statistic("Количество критов",0),
                new Statistic("Всего очков",0),
                new Statistic("Очки за криты",0)
            };
            dataGridView2.DataSource = _statisticsList;

            var source = new BindingSource(_upgradeList, null);

            dataGridView1.DataSource = source;

            _currImg = Properties.Resources._16;
            _critImg = Properties.Resources._17;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            _statisticsList[0].Value++;
            DoClick();
            dataGridView2.Refresh();
        }

        private void DoClick()
        {
            
            var i = _rnd.Next(10);
            var j = _rnd.Next(100);

            if (j == 1 && !_isBoost) { StartBoost(); }

            int booster;

            if (_isBoost)
            {
                booster = 10;

            }
            else
            {
                booster = 1;
            }
            var inc = (i == 1 ? GetClickStrange() * _powerCrit : GetClickStrange()) * booster;

            if (i == 1)
            {
                _statisticsList[6].Value += inc;
                dataGridView2.Refresh();
            }

            _score+=inc;

            _statisticsList[5].Value += inc;
            dataGridView2.Refresh();

            if (i == 1)
            {
                pictureBox1.Image = _critImg;
                _statisticsList[4].Value++;
                dataGridView2.Refresh();
            }

            UpdateScore();
        }

        private double GetClickStrange()
        {
            return _clickPower;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            PictureClickDown();

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            PictureClickUp();
        }

        private void UpdateScore()
        {
            scoreLabel.Text = "Счет: " + _score;
            label3.Text = "Сила клика: "+GetClickStrange();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                {
                    var item = _upgradeList[e.RowIndex];
                    if (_score >= item.Price)
                    {
                        _score -= item.Price;
                        item.Count++;

                        UpdateScore();
                    }
                    _statisticsList[2].Value++;
                    dataGridView2.Refresh();
                    dataGridView1.Refresh();

                    var newPower = _upgradeList.Sum(itm => itm.Power * itm.Count);
                    _clickPower = newPower + 1;
                    break;
                }
                case 1:
                {
                    var item = _upgradeList[e.RowIndex];

                    while (_score >= item.Price)
                    {
                        _score -= item.Price;
                        item.Count++;
                        _statisticsList[2].Value++;
                        dataGridView2.Refresh();
                    }

                    UpdateScore();

                    dataGridView1.Refresh();

                    var newPower = _upgradeList.Sum(itm => itm.Power * itm.Count);
                    _clickPower = newPower + 1;
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_score >= _priceCrit)
            {
                _score -= _priceCrit;
                _priceCrit *= 10;

                _powerCrit *= 1.5;
            }
            label2.Text = "Сила крита: x" + _powerCrit;
            button1.Text = "Цена - " + _priceCrit;
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
            PictureClickDown();
            pictureBox1.Update();
            _statisticsList[1].Value++;
            DoClick();
            dataGridView2.Refresh();
            System.Threading.Thread.Sleep(50);
            PictureClickUp();

        }

        private void PictureClickDown() {
            pictureBox1.Image = _currImg;
            var curLocation = pictureBox1.Location;
            curLocation.X += 5;
            curLocation.Y += 5;

            pictureBox1.Location = curLocation;


            var curSize = pictureBox1.Size;
            curSize.Height -= 10;
            curSize.Width -= 10;

            pictureBox1.Size = curSize;
        }

        private void PictureClickUp() {
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

            if (progressBar1.Value != 100) return;
            timer2.Stop();
            StopBoost();
        }

        private void StartBoost()
        {
            _isBoost = true;
            _currImg = Properties.Resources._18;
            progressBar1.Value = 0;
            timer2.Start();
            BackColor = Color.LightPink;

            _statisticsList[3].Value++;
            dataGridView2.Refresh();
        }

        private void StopBoost()
        {
            _isBoost = false;
            _currImg = Properties.Resources._16;
            progressBar1.Value = 0;
            timer2.Stop();
            BackColor = SystemColors.Control;

        }
    }
}
