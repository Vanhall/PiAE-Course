using ConsecutiveReverseOptimizer;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PiAE_LR4
{
    class Program
    {
        static Vector<double> f(Point P)
        {
            double x1 = P.X[0], x2 = P.X[1], x3 = P.X[2];
            return Vector<double>.Build.DenseOfArray(
                new double[] { 0.096, Math.Log(x1)*0.272, Math.Log(x2)*0.189, Math.Log(x3)*0.191 }
                );
        }

        static IEnumerable<Point> GeneratePlan(int NumPoints)
        {
            var result = new List<Point>(NumPoints * NumPoints * NumPoints);
            double step = 19 / (NumPoints - 1);

            for (int i = 0; i < NumPoints; i++)
                for (int j = 0; j < NumPoints; j++)
                    for (int k = 0; k < NumPoints; k++)
                        result.Add(new Point(1.0 + step * i, 1.0 + step * j, 1.0 + step * k));
            return result;
        }


        static void Main(string[] args)
        {
            int Grid = 20;
            int N = 20;
            
            Console.WriteLine($"Generating Plan for Grid size = {Grid}");
            var CRO = new CRO(f, 4);
            Point[] Plan = GeneratePlan(Grid).ToArray();

            string file = Directory.GetCurrentDirectory() + $"\\{Grid}x{Grid}x{Grid}_N{N}.txt";
            using (StreamWriter output = new StreamWriter(file))
            {
                Console.Write($"Optimizing Plan for N = {N}...");
                var (OptimalPlan, detM) = CRO.OptimisePlan(N, Plan);
                foreach (var X in OptimalPlan) output.WriteLine(X);
                output.WriteLine(detM.ToString(new CultureInfo("en-us")));
                Console.WriteLine("Done!");
            }
            Console.WriteLine("Done!");
        }
    }
}
