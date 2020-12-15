using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2k20
{
    class Day15
    {
        private static readonly List<long> Data = new List<long>() { 1, 0, 16, 5, 17, 4 };

        // Lazy...
        public static void Task1()
        {
            for (int i = Data.Count; i < 2020; i++)
            {
                var iLast = Data.Count - 1;
                var iPrev = Data.SkipLast(1).ToList().LastIndexOf(Data.Last());
                var iNext = 0;
                if (iPrev >= 0) iNext = iLast - iPrev;
                Data.Add(iNext);
            }
            Console.WriteLine($"Task #1 result: {Data.Last()}");
        }

        private static readonly Dictionary<long, int> Data2 = new Dictionary<long, int>() { { 1, 1 }, { 0, 2 }, { 16, 3 }, { 5, 4 }, { 17, 5 } };
        //private static readonly Dictionary<long, int> Data2 = new Dictionary<long, int>()
        //{ { 2, 1 }, { 1, 2 } };

        public static void Task2()
        {
            var lastVal = 4;
            for (int i = Data2.Count+1; i < 30000000; i++)
            {
                var nextVal = 0;
                if (Data2.ContainsKey(lastVal))
                {
                    nextVal = i - Data2[lastVal];
                }
                Data2[lastVal] = i; // Update last-but-one position
                lastVal = nextVal;
            }
            Console.WriteLine($"Task #2 result: {lastVal}");
        }
    }
}
