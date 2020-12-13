using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace aoc2k20
{
    class Day13
    {
        private static readonly string[] Data = File.ReadAllLines("data/13-shuttles - Copy.txt");

        public static void Task1()
        {
            var t0 = long.Parse(Data[0]);
            var lines = Data[1].Split(',').Where(b => b != "x").Select(int.Parse).ToArray();

            var result = 0;
            var t = t0;
            while (result == 0 && t < t0 + lines.Min())
            {
                foreach (var line in lines)
                {
                    if (t % line == 0)
                    {
                        result = line;
                        break;
                    }
                }

                if (result == 0) t++;
            }

            Console.WriteLine($"Task #1 result: {result * (t - t0)}");
        }

        public static void Task2()
        {
            var results = new long[Data.Length];
            for (int d = 1; d < Data.Length; d++)
            {

                var lines = Data[d].Split(',');
                List<Frequency> deps = new List<Frequency>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == "x") continue;
                    var p = long.Parse(lines[i]);
                    deps.Add(new Frequency(p, -i % p)); // Leading offset corresponds to negative phase
                }

                deps = deps.OrderBy(i => i.Period).ToList();

                var pMatch = new List<Frequency>();
                foreach (var dep in deps.Skip(1))
                {
                    var q = CombinePeriods(deps[0], dep);
                    pMatch.Add(q);
                }

                var pComb = pMatch.First();
                foreach (var freq in pMatch.Skip(1))
                {
                    pComb = CombinePeriods(pComb, freq);
                }


                long result = pComb.Period + pComb.Phase;
                Console.WriteLine($"Task #2 result: {result}");
                results[d] = result;
                // 1068781
                // 3417
                // 754018
                // 779210
                // 1261476
                // 1202161486
            }
        }

        public static Frequency CombinePeriods(Frequency a, Frequency b)
        {
            var (gcd, x, y) = EGcd2(a.Period, b.Period);
            long pDiff = a.Phase - b.Phase;
            long pDiv = pDiff / gcd;
            long pRem = pDiff % gcd;

            if (pRem != 0) throw new ApplicationException("Periods will never align");

            long cPer = a.Period / gcd * b.Period;
            long cPhs = (a.Phase - x * pDiv * a.Period) % cPer;
            return new Frequency(cPer, cPhs);
        }

        // Extended Euclidean Algorithm 
        public static (long gcd, long x, long y) EGcd2(long a, long b) 
        { 
            if (a == 0)
            {
                return (b, 0, 1);
            }

            var (gcd,x1,y1) = EGcd2(b % a, a);
            long x = y1 - (b / a) * x1; 
            long y = x1;

            return (gcd, x, y);
        } 


        // Extended Euclidean Algorithm 
        public static long EGcd(long a, long b,  
            out long x, out long y) 
        { 
            if (a == 0) 
            { 
                x = 0; 
                y = 1; 
                return b; 
            }

            long gcd = EGcd(b % a, a, out long x1, out long y1); 
  
            x = y1 - (b / a) * x1; 
            y = x1; 
  
            return gcd; 
        } 
    }

    public class Frequency
    {
        public long Period { get; set; }
        public long Phase { get; set; }

        public Frequency(long period, long phase)
        {
            Period = period;
            Phase = phase;
        }
    }
}