using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    public abstract class IImpactPoint
    {
        public float X; 
        public float Y;
        public int Particlescounter;
        public int alpha;// прозрачность

        public abstract void ImpactParticle(Particle particle);
      
        // базовый класс для отрисовки точечки
        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Orange),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }
    public class GravityPoint : IImpactPoint
    {
        public int Power = 100; // сила притяжения
        public override void Render(Graphics g)
        {
            g.FillEllipse(
                   new SolidBrush(Color.FromArgb(alpha, Color.Orange)),
                   X - Power / 2,
                   Y - Power / 2,
                   Power,
                   Power
               ); ;
            g.DrawEllipse(
                   new Pen(Color.White),
                   X - Power / 2,
                   Y - Power / 2,
                   Power,
                   Power
               ); ;
            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // обязательно выносим текст и шрифт в переменные
            var font = new Font("Times new roman", 15);
            var cou = $"{Particlescounter}";
            // вызываем MeasureString, чтобы померить размеры текста
            var size = g.MeasureString(cou, font);
          
            // ну и текст рисую уже на базе переменных
            g.DrawString(
                cou,
                font,
                new SolidBrush(Color.White),
                X,
                Y,
                stringFormat
            );
        }
        // а сюда по сути скопировали с минимальными правками то что было в UpdateState
        public override void ImpactParticle(Particle particle)
        {
            
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double r = Math.Sqrt(gX * gX + gY * gY); // считаем расстояние от центра точки до центра частицы           
            if (r + particle.Radius < Power / 2) // если частица оказалось внутри окружности
            {
                particle.Life = 0;
                
                if (particle.Life <= 0)
                {
                    Particlescounter++;             
                }
                if (alpha < 255)
                {
                    alpha++;
                }
            }

        }

    }  
}
