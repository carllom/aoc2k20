using System;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day9
    {
        private static readonly long[] Data = File.ReadAllLines("data/9-encoding.txt").Select(long.Parse).ToArray();
        public static void Task1()
        {
            var result = 0L;

            for (int b = 25; b < Data.Length; b++)
            {
                if (Validate(b)) continue;
                result = b;
                break;
            }

            Console.WriteLine($"Task #1 result: {Data[result]}");
        }

        private static bool Validate(int pos)
        {
            for (int i1 = 1; i1 < 25; i1++) // 1-24th number
            {
                for (int i2 = i1+1; i2 < 26; i2++) // 2-25th number
                {
                    if (Data[pos - i1] + Data[pos - i2] == Data[pos]) return true;
                }
            }
            return false;
        }

        public static void Task2()
        {
            var iErr = 640; // Index from last result 
            var lohi = Validate2(iErr);

            Console.WriteLine($"Task #2 result: {lohi.lo}..{lohi.hi} = {lohi.lo + lohi.hi}");
        }

        private static (long lo, long hi) Validate2(int iErr)
        {
            var target = Data[iErr]; // last result
            for (int i0 = 0; i0 < iErr; i0++)
            {
                long acc = 0, lo = long.MaxValue, hi = long.MinValue;
                for (int i = i0; i < iErr; i++)
                {
                    var d = Data[i];
                    lo = Math.Min(lo, d);
                    hi = Math.Max(hi, d);
                    acc += d;
                    if (acc == target) return (lo, hi);
                    if (acc > target) break;
                }
            }
            return (0,0);
        }
    }
}
