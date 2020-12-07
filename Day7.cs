using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day7
    {
        private static string[] data = File.ReadAllLines("data/7-luggage.txt");
        private static Dictionary<string, List<Tuple<int,string>>> rules = new Dictionary<string, List<Tuple<int,string>>>();

        private static void ParseData()
        {
            foreach (var rule in data)
            {
                var r = rule.Split(new string[] { " bags contain ", " bags", " bag", ", ", ".", "no other" }, StringSplitOptions.RemoveEmptyEntries);
                var key = r[0];
                var rn = new List<Tuple<int,string>>();
                for (int i = 1; i < r.Length; i++)
                {
                    var ws = r[i].IndexOf(' ');
                    var qty = int.Parse(r[i].Substring(0, ws));
                    var type = r[i].Substring(ws+1);
                    rn.Add(new Tuple<int, string>(qty, type));
                }
                rules[key] = rn;
            }
        }

        private static List<string> AllKeysContaining(string key) => rules.Where(r => r.Value.Any(v => v.Item2 == key)).Select(r => r.Key).ToList();

        public static void Task1()
        {
            ParseData();
            var result = AllKeysContaining("shiny gold");
            var keys = result;
            while (true)
            {
                var newKeys = new List<string>();
                foreach (var key in keys)
                {
                    newKeys.AddRange(AllKeysContaining(key).Where(k => !result.Contains(k)));
                }
                if (newKeys.Count == 0) break;
                keys = newKeys.Distinct().ToList();
                result.AddRange(keys);
            }
            Console.WriteLine($"Task #1 result: {result.Count()}");
        }

        public static int NumBags(string key)
        {
            if (!rules.ContainsKey(key)) return 1;
            var count = 1;
            foreach (var item in rules[key])
            {
                count += item.Item1 * NumBags(item.Item2);
            }
            return count;
        }

        public static void Task2()
        {
            ParseData();
            var result = NumBags("shiny gold");
            Console.WriteLine($"Task #2 result: {result}");
        }
    }
}
