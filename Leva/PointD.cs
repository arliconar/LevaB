using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Leva
{
    public class PointD
    {
        public double X;
        public double Y;

        public PointD(double X,double Y)
        {
            this.X=X;
            this.Y=Y;
        }
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
