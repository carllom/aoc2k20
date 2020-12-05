using System;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day5
    {
        private static string[] data = File.ReadAllLines("data/5-boardingpass.txt");

        private static int ParseSeating(string seating)
        {
            var coord = 0;
            foreach (var p in seating)
            {
                coord <<= 1;
                if (p == 'B' || p == 'R') coord |= 1;
            }
            return coord;
        }

        public static void Task1()
        {
            var q = data.Select(s => ParseSeating(s)).OrderByDescending(s => s).First();
            Console.WriteLine($"Task #1 - last seat: {q}");
        }
        public static void Task2()
        {
            var freeSeatAt = 0;
            var q = data.Select(s => ParseSeating(s)).OrderBy(s => s).ToArray();
            var lastseat = q[0];
            for (int i = 1; i < q.Length; i++)
            {
                if (q[i] == lastseat + 1) { lastseat = q[i]; } else { freeSeatAt = lastseat + 1; break; }
            }
            Console.WriteLine($"Task #2 - free seat: {freeSeatAt}");
        }
    }
}
