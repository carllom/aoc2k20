using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2k20
{
    class Day2
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

        private static IEnumerable<PasswordItem> passwords = File.ReadAllLines("data/2-passwords.txt").Select(r => new PasswordItem(r));
        public static void Task1()
        {
            var valid = 0;
            foreach (var item in passwords)
            {
                var numberMatching = item.Password.Count(c => c == item.Char);
                if (numberMatching < item.Min || numberMatching > item.Max)
                {
                    Console.WriteLine($"Mismatch for {item.RawData}, actual = {numberMatching}");
                }
                else { valid++; }
            }
            Console.WriteLine($"Valid passwords: {valid}");
        }
        public static void Task2()
        {
            var valid = 0;
            foreach (var p in passwords)
            {
                bool m1 = p.Password[p.Min - 1] == p.Char;
                bool m2 = p.Password[p.Max - 1] == p.Char;
                if (m1 ^ m2) 
                    valid++;
                else
                    Console.WriteLine($"Mismatch for {p.RawData}, actual: {m1}, {m2}");
            }
            Console.WriteLine($"Valid passwords: {valid}");
        }
    }
}
