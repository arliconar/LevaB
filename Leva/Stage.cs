using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;

namespace Leva
{
    public class Stage
    {
        public const double pi = Math.PI;
        public enum Type
        {
            Low,
            Rising,
            High,
            Falling,

        }
        public List<Point> pointscurve = new List<Point>();
        public List<PointF> Pointsg = new List<PointF>();
        public List<PointF> points = new List<PointF>();
        public List<PointF> pointspos = new List<PointF>();
        public List<PointF> pointsvel = new List<PointF>();
        public List<PointF> pointsacel = new List<PointF>();
        public List<PointF> pointsjerk = new List<PointF>();

        public double degStart = 0;

        public double degEnd = 90;
        Type type = Type.Low;

        private double h = 1;
        public double off = 0.5;
        public double step = 0.01;
        public float scale = 90;
        public float startx=0;
        private double[] valpol = { 1, 6, 15, 20, 15, 6, 1 };

        public void seth(float h,float off)
        {
            this.h = h;
            this.off = off;
            switch (this.type)
            {
                case Type.Rising:
                    
                    scalepoints(h,off);
                    Pointsg[Pointsg.Count - 1] = new PointF(Pointsg[Pointsg.Count - 1].X, h);
                    Pointsg[0] = new PointF(Pointsg[0].X, off);
                    break;

               case Type.High:
               case Type.Low:
                    this.h = h;
                    this.off = off;
                    break;
                
                case Type.Falling:
                    scalepoints(h,off);
                    Pointsg[0] = new PointF(Pointsg[0].X, h);
                    Pointsg[Pointsg.Count - 1] = new PointF(Pointsg[Pointsg.Count - 1].X, off);

                    break;
            }
              calculate();
        }

        public void scaleaxis(float xmin, float xmax)
        {
            if(this.type == Stage.Type.Rising || this.type==Stage.Type.Falling)
            {                
                float scale = xmax - xmin;
                float minx = Pointsg.First().X;
                float maxx = Pointsg.Last().X;
                float scale2 = maxx - minx;
                Pointsg[0]= new PointF(xmin,Pointsg.First().Y);
                Pointsg[Pointsg.Count-1] = new PointF(xmax, Pointsg.Last().Y);
                for (int i = 1; i < Pointsg.Count-1; i++)
                {
                    float nx = (((Pointsg[i].X-minx) / scale2)*scale)+xmin;
                    Pointsg[i] = new PointF(nx, Pointsg[i].Y);
                }

            }
                degStart = xmin;
                degEnd = xmax;
        }

        private void scalepoints(float h,float off)
        {
            float maxy = 0;
            for (int i = 0; i < Pointsg.Count; i++)
            {
                if (Pointsg[i].Y > maxy)
                    maxy = Pointsg[i].Y;
            }
            for (int i = 0; i < Pointsg.Count; i++)
            {
                 Pointsg[i] = new PointF(Pointsg[i].X,off+((h-off) * (Pointsg[i].Y / maxy)));
            }

        }

        public Stage(Type type)
        {
          
            this.type = type;
            this.Pointsg.Clear();
        }
        
        public void setPoints(List <Point> points)
        {
            this.pointscurve.Clear();
            this.pointscurve = points;
        }

        public void calculate()
        {

            switch (this.type)
            {
                case Type.Low:
                    pointspos = calculateLow();
                    break;
                case Type.Rising:
                case Type.Falling:
                    pointspos = calcbezier();
                    pointsvel = calcvel();
                    pointsacel = calcacel();
                    pointsjerk = calcjerk();
                    break;
                case Type.High:
                    pointspos = calculateHigh();
                    break;
            }
           

        }

        private double stepCalculation()
        {

            double inc = (degEnd - degStart) / step;

            return ((double)2) / inc;
        }
        private List<PointF> calculateLow()
        {
            double scale = degEnd - degStart;
            List<PointF> pointf = new List<PointF>();
            for (double i = 0; i <= 1; i += step)
            {
                pointf.Add(new PointF((float)(degStart + (i*scale)), (float)off));
            }
            return pointf;
        }
        private List<PointF> calculateHigh()
        {
            List<PointF> pointf = new List<PointF>();
            double scale = degEnd - degStart;

            for (double i = 0; i <= 1; i += step)
            {
                pointf.Add(new PointF((float)(degStart + (i * scale)), (float)h));
            }
            return pointf;
        }
        public List<PointF> calcbezier()
        {
            List<PointF> points = new List<PointF>();
            for (double t = 0; t <= 1; t += step)
            {
                double px = valpol[0] * Pointsg[0].X * Math.Pow((1 - t), 6) + valpol[1] * Pointsg[1].X * Math.Pow((1 - t), 5) * t + valpol[2] * Pointsg[2].X * Math.Pow((1 - t), 4) * Math.Pow(t, 2) + valpol[3] * Pointsg[3].X * Math.Pow((1 - t), 3) * Math.Pow(t, 3) + valpol[4] * Pointsg[4].X * Math.Pow((1 - t), 2) * Math.Pow(t, 4) + valpol[5] * Pointsg[5].X * Math.Pow((1 - t), 1) * Math.Pow(t, 5) + valpol[6] * Pointsg[6].X * Math.Pow(t, 6);////
                double py = valpol[0] * Pointsg[0].Y * Math.Pow((1 - t), 6) + valpol[1] * Pointsg[1].Y * Math.Pow((1 - t), 5) * t + valpol[2] * Pointsg[2].Y * Math.Pow((1 - t), 4) * Math.Pow(t, 2) + valpol[3] * Pointsg[3].Y * Math.Pow((1 - t), 3) * Math.Pow(t, 3) + valpol[4] * Pointsg[4].Y * Math.Pow((1 - t), 2) * Math.Pow(t, 4) + valpol[5] * Pointsg[5].Y * Math.Pow((1 - t), 1) * Math.Pow(t, 5) + valpol[6] * Pointsg[6].Y * Math.Pow(t, 6);////
                points.Add(new PointF((float)px, (float)py));
            }
            return points;
        }
        public List<PointF> calcacel()
        {
            List<PointF> points = new List<PointF>();
            for (double t = 0; t <= 1; t += step)
            {
                double aay = 30 * Pointsg[0].Y * Math.Pow((1 - t), 4);
                double bay = 60 * Pointsg[1].Y * Math.Pow((1 - t),3) * (-1 + 3 * t);
                double cay = 30 * Pointsg[2].Y * Math.Pow((1 - t), 2) * (1 - 10 * t + 15 * Math.Pow(t, 2));
                double day = -120 * Pointsg[3].Y * t * (-1 + 6 * t - 10 * Math.Pow(t,2) + 5 * Math.Pow(t, 3));
                double eay = 30 * Pointsg[4].Y * Math.Pow(t,2) * (6 - 20 * t + 15 * Math.Pow(t,2));
                double fay = -60 * Pointsg[5].Y * Math.Pow(t, 3) * (-2 + 3 * t);
                double gay = 30 * Pointsg[6].Y * Math.Pow(t, 4);
                double phiya = aay + bay + cay + day + eay + fay + gay;
                points.Add(new PointF(startx+(float)t*scale, (float)phiya));
            }
            return points;
        }
        public List<PointF> calcjerk()
        {
            List<PointF> points = new List<PointF>();
            for (double t = 0; t <= 1; t += step)
            {
                double ajy = -120 * Pointsg[0].Y * Math.Pow((1 - t),3);
                double bjy = -360 * Pointsg[1].Y * Math.Pow((1 - t), 2) * (-1 + 2 * t);
                double cjy = 360 * Pointsg[2].Y * (-1 + 6 * t - 10 * Math.Pow(t,2) + 5 * Math.Pow(t,3));
                double djy = -120 * Pointsg[3].Y * (-1 + 12 * t - 30 * Math.Pow(t,2) + 20 * Math.Pow(t, 3));
                double ejy = 360 * Pointsg[4].Y * t * (1 - 5 * t + 5 * Math.Pow(t,2)); 
                double fjy = 360 * Pointsg[5].Y * (1 - 2 * t) * Math.Pow(t, 2);
                double gjy = 120 * Pointsg[6].Y * Math.Pow(t, 3);
                double phiyj = ajy + bjy + cjy + djy + ejy + fjy + gjy;
                points.Add(new PointF(startx + (float)t * scale, (float)phiyj));
            }
            return points;
        }
        public List<PointF> calcvel()
        {

            List<PointF> points = new List<PointF>();
            for (double t = 0; t <= 1; t += step)
            {
               double avy = -6 * Pointsg[0].Y * Math.Pow((1 - t),5);
               double bvy = -6 * Pointsg[1].Y * Math.Pow((1 - t),4) * (-1 + 6 * t);
               double cvy = -30 * Pointsg[2].Y * Math.Pow((1 - t),3) * t * (-1 + 3 * t);
               double dvy = -60 * Pointsg[3].Y * Math.Pow((1 - t),2) * Math.Pow(t,2) * (-1 + 2 * t);
               double evy = 30 * Pointsg[4].Y * Math.Pow(t,3) * (2 - 5 * t + 3 * Math.Pow(t,2));
               double fvy = 6 * Pointsg[5].Y * (5 - 6 * t) * Math.Pow(t,4);
               double gvy = 6 * Pointsg[6].Y * Math.Pow(t,5);
               double phiyv = avy + bvy + cvy + dvy + evy + fvy + gvy;
               points.Add(new PointF(startx+(float)t*scale,(float)phiyv));
            }
            return points;
        }
        private double derbez(double V, double Pa, int n1, int n2, double t)
        {
            int s = -1;
            if (n1 == 0)
                s = 1;
            return s * Pa * V * (n1 * Math.Pow((1 - t), (n1 - 1)) * Math.Pow(t, 2) + n2 * Math.Pow(t, (n2 - 1)) * Math.Pow((1 - t), n1));
        }

        
    }
}
