using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaNigaGame.Folders.Classes
{
    internal class Point
    {
        private double x;
        private double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double GetX()
        {
            return x;
        }

        public void SetX(double value)
        {
            x = value;
        }

        public double GetY()
        {
            return y;
        }

        public void SetY(double value)
        {
            y = value;
        }

        public double Distance(Point other)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
