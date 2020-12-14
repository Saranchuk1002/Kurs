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
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter; // добавили эмиттер
        GravityPoint point1; // добавил поле под первую точку
        GravityPoint point2; // добавил поле под вторую точку
        GravityPoint newpoint;
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            tbDirection.Value = 270;
            tbSpreading.Value = 100;
            trackBar1.Value = 10;
            trackBar2.Value = 10;
            picDisplay.MouseWheel += PicDisplay_MouseWheel;
            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                Direction = 270,
                Spreading = 100,
                SpeedMin = 1,
                SpeedMax = 10,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 10,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 6,
            };

            emitters.Add(this.emitter); // все равно добавляю в список emitters, чтобы он рендерился и обновлялся

            point1 = new GravityPoint
            {
                X = picDisplay.Width / 2 + 100,
                Y = picDisplay.Height / 2,
            };
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
        

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = tbDirection.Value; // направлению эмиттера присваиваем значение ползунка 
            lblDirection.Text = $"{tbDirection.Value}°"; // добавил вывод значения
        }

        private void tbSpreading_Scroll(object sender, EventArgs e)
        {
            emitter.Spreading = tbSpreading.Value;
            label8.Text = $"{tbSpreading.Value}°";
        }

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
        private void PicDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0 && point1.Power > 0)
            {
                foreach (var p in emitter.impactPoints)
                {  
                    if (p is GravityPoint) // так как impactPoints не обязательно содержит поле Power, надо проверить на тип 
                    {
                        // если гравитон то меняем силу
                        (p as GravityPoint).Power -= 5;
                    }
                }
            }
            if (e.Delta > 0 && point1.Power < 100)
            {
                foreach (var p in emitter.impactPoints)
                {
                    if (p is GravityPoint) // так как impactPoints не обязательно содержит поле Power, надо проверить на тип 
                    {
                        // если гравитон то меняем силу
                        (p as GravityPoint).Power +=5 ;
                    }
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = trackBar1.Value;
            label9.Text = $"{trackBar1.Value}°";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            emitter.SpeedMax = trackBar2.Value;
            emitter.SpeedMin = trackBar2.Value;
            label10.Text = $"{trackBar2.Value}°";
        }
        
    }
}