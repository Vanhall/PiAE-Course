using ConsecutiveReverseOptimizer;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PiAE_LR3
{
    class Program
    {
        static Vector<double> f(Point P)
        {
            double x1 = P.X[0], x2 = P.X[1];
            double x1s = x1 * x1, x2s = x2 * x2;
            double x1c = x1s * x1, x2c = x2s * x2;
            return Vector<double>.Build.DenseOfArray(
                new double[] {
                1, x1, x2, x1 * x2,
                x1s, x2s, x1s * x2s,
                x1c, x2c, x1c * x2c,
                }
                );
        }
        
        static void Main(string[] args)
        {
            int[] Ns = { 20, 25, 30, 35, 40 };
            int[] Grids = { 30, 40 };
            
            
            foreach (var Grid in Grids)
            {
                Console.WriteLine($">>>----- Generating Plan for Grid size = {Grid} -----<<<");
                var CRO = new CRO(f, 10);
                Point[] Plan = CRO.GeneratePlan(Grid).ToArray();
                foreach (var N in Ns)
                {
                    string file = Directory.GetCurrentDirectory() + $"\\{Grid}x{Grid}_N{N}.txt";
                    using (StreamWriter output = new StreamWriter(file))
                    {
                        Console.Write($"Optimizing Plan for N = {N}...");
                        var (OptimalPlan, detM) = CRO.OptimisePlan(N, Plan);
                        foreach (var X in OptimalPlan) output.WriteLine(X);
                        output.WriteLine(detM.ToString(new CultureInfo("en-us")));
                        Console.WriteLine("Done!");
                    }
                }
            }
            Console.WriteLine("Done!");
        }
    }
}
