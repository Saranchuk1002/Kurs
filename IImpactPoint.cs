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
        public int SmallParticlescounter = 0;//счетчик под маленькие частицы
        public int MediumParticlescounter = 0;//счетчик под средние частицы
        public int BigParticlescounter = 0;//счетчик под большие частицы
        public int alpha;// прозрачность

        public abstract void ImpactParticle(Particle particle);
      
        // базовый класс для отрисовки точки
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
        //рисуем счетчики
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

            g.DrawString(
                $"Маленькие {SmallParticlescounter}\n Средние {MediumParticlescounter}\n Большие {BigParticlescounter}",
                new Font("Times new roman", 10),
                new SolidBrush(Color.White),
                X,
                Y,
                stringFormat
            );
        }
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
                    
                    if (particle.Radius < 4)
                    {
                        SmallParticlescounter++;
                    }
                    else
                    {
                        if (particle.Radius > 8)
                        {
                            MediumParticlescounter++;
                        }
                        else
                        {
                            BigParticlescounter++;
                        }
                    }
                }
                if (alpha < 255)
                {
                    alpha++;
                }
            }

        }

    }  
}
