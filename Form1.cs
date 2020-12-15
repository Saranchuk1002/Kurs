using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static kurs.Particle;

namespace kurs
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();//создаем лист
        Emitter emitter; // добавили эмиттер
        GravityPoint point1; // поле под первую точку
        GravityPoint point2; // поле под вторую точку
        GravityPoint newpoint; // поле под новые точки
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            tbDirection.Value = 270;//значение ползунка с направлением
            tbSpreading.Value = 100;//значение ползунка с разбросом
            trackBar1.Value = 10;//значение ползунка с количеством частиц
            trackBar2.Value = 10;//значение ползунка со скоростью частиц
            picDisplay.MouseWheel += PicDisplay_MouseWheel;
            this.emitter = new Emitter // создаем эмиттер и привязываем его к полю emitter
            {
                Direction = 270, //направление
                Spreading = 100, //разброс
                SpeedMin = 1, //начальная минимальная скорость движения частиц
                SpeedMax = 10, //начальная максимальная скорость движения частицы
                ColorFrom = Color.Gold,//начальный цвет частиц
                ColorTo = Color.FromArgb(0, Color.Red),//конечный цвет частиц
                ParticlesPerTick = 10,//количество частиц за тик
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 6,
            };

            emitters.Add(this.emitter); // добавляем emitters в список, чтобы он рендерился и обновлялся
            //создаем первый счетчик
            point1 = new GravityPoint
            {
                X = picDisplay.Width / 2 + 100,
                Y = picDisplay.Height / 2,
            };
            //создаем второй счетчик
            point2 = new GravityPoint
            {
                X = picDisplay.Width / 2 - 100,
                Y = picDisplay.Height / 2,
            };

            // привязываем поля к эмиттеру
            emitter.impactPoints.Add(point1);
            emitter.impactPoints.Add(point2);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState(); // каждый тик обновляем систему

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g); // рендерим систему
            }
        
            picDisplay.Invalidate();
        }

        // направлению эмиттера присваиваем значение ползунка 
        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = tbDirection.Value; 
            lblDirection.Text = $"{tbDirection.Value}°"; // добавили вывод значения
        }
        // разбросу эмиттера присваиваем значение ползунка 
        private void tbSpreading_Scroll(object sender, EventArgs e)
        {
            emitter.Spreading = tbSpreading.Value;
            label8.Text = $"{tbSpreading.Value}°";
        }
        //при нажатии ЛКМ создаем новый счетчик
        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                newpoint = new GravityPoint
                {
                    X = e.X,
                    Y = e.Y,
                };
                emitter.impactPoints.Add(newpoint);
            }
            //при нажатии ПКМ удаляем выбранный счетчик
            else if (e.Button == MouseButtons.Right)
            {
                {
                    int xMouse = e.X;
                    int yMouse = e.Y;

                    for (int i=0; i < emitter.impactPoints.Count(); i++)
                    {
                        var g = Graphics.FromImage(picDisplay.Image);

                        float gX = emitter.impactPoints[i].X - xMouse;
                        float gY = emitter.impactPoints[i].Y - yMouse;

                        double r = Math.Sqrt(gX * gX + gY * gY); //расстояние от центра точки до мышки
                        if (r < emitter.impactPoints[i].Power / 2)
                        {
                            emitter.impactPoints.RemoveAt(i);
                            
                            break;
                        }
                    }
                }
            }
        }
        //увеличиваем или уменьшаем радиус счетчика колесиком мыши, при наведении на него
        private void PicDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            int xMouse = e.X;
            int yMouse = e.Y;

            for (int i = 0; i < emitter.impactPoints.Count(); i++)
            {
                var g = Graphics.FromImage(picDisplay.Image);

                float gX = emitter.impactPoints[i].X - xMouse;
                float gY = emitter.impactPoints[i].Y - yMouse;

                double r = Math.Sqrt(gX * gX + gY * gY); //расстояние от центра точки до мышки
                if (r < emitter.impactPoints[i].Power / 2)
                {
                    if (e.Delta > 0)
                    {
                        if (emitter.impactPoints[i].Power < 200)
                            emitter.impactPoints[i].Power += 5;
                    }
                    else if (e.Delta < 0)
                    {
                        if (emitter.impactPoints[i].Power > 10)
                            emitter.impactPoints[i].Power -= 5;
                    }
                }
            }

        }// количеству частиц за тик присваиваем значение ползунка
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = trackBar1.Value;
            label9.Text = $"{trackBar1.Value}";
        }
        // скорости эмиттера присваиваем значение ползунка
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            emitter.SpeedMax = trackBar2.Value;
            emitter.SpeedMin = trackBar2.Value;
            label10.Text = $"{trackBar2.Value}";
        }
        
    }
}