using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsecutiveReverseOptimizer
{
    public class CRO
    {
        public int Dim { get; private set; }
        
        Func<Point, Vector<double>> f;

        public CRO(Func<Point, Vector<double>> Function, int Dimension)
        {
            f = Function;
            Dim = Dimension;
        }
        
        private double d(Point X, Matrix<double> M)
        {
            var F = f(X).ToColumnMatrix();
            return (F.Transpose() * M.Inverse() * F)[0, 0];
        }

        public IEnumerable<Point> GeneratePlan(int NumPoints)
        {
            var result = new List<Point>(NumPoints * NumPoints);
            double step = 2.0 / (NumPoints - 1);

            for (int i = 0; i < NumPoints; i++)
                for (int j = 0; j < NumPoints; j++)
                    result.Add(new Point(-1.0 + step * i, -1.0 + step * j));
            return result;
        }

        private Matrix<double> CalcM(IEnumerable<Point> Plan)
        {
            int N = Plan.Count();
            double P = 1.0 / N;
            var M = Matrix<double>.Build.Dense(Dim, Dim, 0.0);

            foreach (var X in Plan)
            {
                var F = f(X).ToColumnMatrix();
                M += P * F * F.Transpose();
            }
            return M;
        }

        public (IEnumerable<Point>, double) OptimisePlan(int N, IEnumerable<Point> Plan)
        {
            var CurrentPlan = Plan.ToList();
            Matrix<double> M = null;

            for (int s = Plan.Count(); s > N; s--)
            {
                M = CalcM(CurrentPlan);
                int minIndex = 0;
                double min = d(CurrentPlan[minIndex], M);
                for (int i = 1; i < s; i++)
                {
                    double funcValue = d(CurrentPlan[i], M);
                    if (min > funcValue)
                    {
                        min = funcValue;
                        minIndex = i;
                    }
                }
                CurrentPlan.RemoveAt(minIndex);
            }
            return (CurrentPlan, M.Determinant());
        }
    }
}
