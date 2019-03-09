using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ConsecutiveReverseOptimizer
{
    public struct Point
    {
        public double[] X;

        public Point(double X1, double X2)
        {
            X = new double[] { X1, X2 };
            ci = new CultureInfo("en-us");
        }

        public Point(double X1, double X2, double X3)
        {
            X = new double[] { X1, X2, X3 };
            ci = new CultureInfo("en-us");
        }

        public Point(IEnumerable<double> X)
        {
            this.X = X.ToArray();
            ci = new CultureInfo("en-us");
        }

        private CultureInfo ci;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < X.Count() - 1; i++) sb.Append($"{X[i].ToString(ci)}\t");
            sb.Append(X[X.Count() - 1].ToString(ci));
            return sb.ToString();
        }
    }
}
