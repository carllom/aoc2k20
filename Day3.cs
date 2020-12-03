using System;
using System.IO;
using System.Text.RegularExpressions;

namespace aoc2k20
{
    class Day3
    {
        //11-13 m: mmmmmgmmmmnmw
        private class PasswordItem
        {
            private static Regex pwData = new Regex("(?<min>\\d+)\\s*-\\s*(?<max>\\d+)\\s*(?<char>\\w)\\s*:\\s*(?<pwd>\\w+)");
            public int Min { get; }
            public int Max { get; }
            public char Char { get; }
            public string Password { get; }

            public string RawData { get; }

            public PasswordItem(string raw)
            {
                RawData = raw;
                var m = pwData.Match(raw);
                if (m.Success)
                {
                    Min = int.Parse(m.Groups["min"].Value);
                    Max = int.Parse(m.Groups["max"].Value);
                    Char = m.Groups["char"].Value[0];
                    Password = m.Groups["pwd"].Value;
                }
            }
        }

        private static string[] map = File.ReadAllLines("data/3-trees.txt");
        public static void Task1()
        {
            var trees = 0;
            int x = 0, y = 0;
            while (y < map.Length)
            {
                if (map[y][x % map[y].Length] == '#') trees++;
                x += 3;
                y += 1;
            }
            Console.WriteLine($"Number of trees: {trees}");
        }
        public static void Task2()
        {
            var total = Slopes(1, 1) * 250 * Slopes(5, 1) * Slopes(7, 1) * Slopes(1, 2);
            Console.WriteLine($"Total Number of trees: {total}");
        }

        private static int Slopes(int xd, int yd)
        {
            var trees = 0;
            int x = 0, y = 0;
            while (y < map.Length)
            {
                if (map[y][x % map[y].Length] == '#') trees++;
                x += xd;
                y += yd;
            }
            return trees;
        }
    }
}
