using System;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day6
    {
        private static string[] data = File.ReadAllLines("data/6-customsdeclaration.txt");

        public static int UniqueQuestions(ref int index)
        {
            string q = "";
            while (index<data.Length && !string.IsNullOrWhiteSpace(data[index]))
            {
                q += data[index];
                index++;
            }
            index++; // skip empty line
            return q.Distinct().Count();
        }

        public static void Task1()
        {
            var result = 0;
            int index = 0;
            while (index < data.Length)
            {
                result += UniqueQuestions(ref index);
            }

            Console.WriteLine($"Task #1 result: {result}");
        }

        public static int SameQuestions(ref int index)
        {
            string q = "";
            var numpeople = 0;
            while (index < data.Length && !string.IsNullOrWhiteSpace(data[index]))
            {
                q += data[index];
                numpeople++;
                index++;
            }
            index++; // skip empty line

            return q.GroupBy(x => x).Where(x => x.Count() == numpeople).Count();
        }
        public static void Task2()
        {
            var result = 0;
            int index = 0;
            while (index < data.Length)
            {
                result += SameQuestions(ref index);
            }
            Console.WriteLine($"Task #2 result: {result}");
        }
    }
}
