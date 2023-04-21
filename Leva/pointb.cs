using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Leva
{
    public class pointb
    {
        public enum typeP
        {
            None,
            Rising,
            Falling,
            sLow,
            sHigh
        }
        public Point fixedcenter
        {
            get
            {

                return new Point(center.X, 500 - center.Y);

            }
        }
        private int height = 10;
        public bool fixedpoint = false;
        public bool mousein = false;
        public string name = "";
        public Point center = new Point(0, 0);
        public Font font = new Font("Arial", 10);
        public typeP type = typeP.None;
       
        
        
        public pointb(Point _center, int width, string name,typeP type, bool fixedpoint = false)
        {
            height = width;
            center = _center;
            this.name = name;
            this.fixedpoint = fixedpoint;
            this.type= type;
        }

    
        public pointb(int xcenter,int ycenter, int width)
        {
            height = width;
            center = new Point(xcenter,ycenter);
        }
        public Rectangle rect
        {
            get
            {
                Rectangle r = new Rectangle(center.X,center.Y,height,height);
                return r;
            }
                    
        }
        public bool checkMousein(Point p)
        {
            
            if (p.X < (center.X - height) || p.X > (center.X + height) || p.Y < (center.Y - height) || p.Y > (center.Y + height))
            {
                mousein = false;
                return false;
            }
                
            else
            {
                mousein = true;
                return true;
            }
                

        }
    }
}
