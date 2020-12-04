using System;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day4
    {
        private static string[] data = File.ReadAllLines("data/4-passports.txt");
        public static void Task1()
        {
            int index = 0;
            var result = 0;
            while (index < data.Length)
            {
                if (ValidPassport(ref index)) result++;
            }
            Console.WriteLine($"Task #1 result: {result}");
        }

        public static bool ValidPassport(ref int index)
        {
            byte valid = 0x80; // pre-validate cid 
            while (!string.IsNullOrWhiteSpace(data[index]))
            {
                var fields = data[index++].Split();
                foreach (var field in fields)
                {
                    switch (field.Split(':')[0])
                    {
                        case "byr": // (Birth Year)
                            valid |= 0x01; break;
                        case "iyr": //(Issue Year)
                            valid |= 0x02; break;
                        case "eyr": //(Expiration Year)
                            valid |= 0x04; break;
                        case "hgt": //(Height)
                            valid |= 0x08; break;
                        case "hcl": //(Hair Color)
                            valid |= 0x10; break;
                        case "ecl": //(Eye Color)
                            valid |= 0x20; break;
                        case "pid": //(Passport ID)
                            valid |= 0x40; break;
                        case "cid": //(Country ID)
                            valid |= 0x80; break;
                    }
                }
            }
            index++; // skip empty line
            return valid == 0xFF;
        }

        public static void Task2()
        {
            int index = 0;
            var result = 0;
            while (index < data.Length)
            {
                if (ValidPassport2(ref index)) result++;
            }
            Console.WriteLine($"Task #2 result: {result}");
        }

        public static bool ValidRange(string tok, int low, int high)
        {
            var dat = 0;
            return (int.TryParse(tok, out dat) && dat >= low && dat <= high);
        }

        public static bool ValidPassport2(ref int index)
        {
            byte valid = 0x80; // pre-validate cid 
            while (!string.IsNullOrWhiteSpace(data[index]))
            {
                var fields = data[index++].Split();
                foreach (var field in fields)
                {
                    var toks = field.Split(':');
                    switch (toks[0])
                    {
                        case "byr": // (Birth Year)
                            if (toks[1].Length == 4 && ValidRange(toks[1], 1920, 2020))
                                valid |= 0x01;
                            break;
                        case "iyr": //(Issue Year)
                            if (toks[1].Length == 4 && ValidRange(toks[1], 2010, 2020))
                                valid |= 0x02;
                            break;
                        case "eyr": //(Expiration Year)
                            if (toks[1].Length == 4 && ValidRange(toks[1], 2020, 2030))
                                valid |= 0x04;
                            break;
                        case "hgt": //(Height)
                            if (toks[1].EndsWith("cm") && ValidRange(toks[1].Remove(toks[1].Length - 2), 150, 193) ||
                                toks[1].EndsWith("in") && ValidRange(toks[1].Remove(toks[1].Length - 2), 59, 76))
                                valid |= 0x08;
                            break;
                        case "hcl": //(Hair Color)
                            if (toks[1][0] == '#' && toks[1].Length == 7 && int.TryParse(toks[1].Substring(1), System.Globalization.NumberStyles.HexNumber, null, out int _))
                                valid |= 0x10;
                            break;
                        case "ecl": //(Eye Color)
                            var colors = new[] {"amb", "blu","brn","gry","grn","hzl","oth"};
                            if (colors.Contains(toks[1]))
                                valid |= 0x20; 
                            break;
                        case "pid": //(Passport ID)
                            if (toks[1].Length == 9 && int.TryParse(toks[1], out int _))
                                valid |= 0x40;
                            break;
                        case "cid": //(Country ID)
                            valid |= 0x80; break;
                    }
                }
            }
            index++; // skip empty line
            return valid == 0xFF;
        }
    }
}
