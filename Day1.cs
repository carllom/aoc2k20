using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace aoc2k20
{
    class Day1
    {
        public static void Task1()
        {
            var expenses = File.ReadAllLines("data/1-expenses.txt").Select(x => int.Parse(x)).ToArray();
            int x1 = 0, x2 = 0;
            for (int i1 = 0; i1 < expenses.Length; i1++)
            {
                x1 = expenses[i1];
                for (int i2 = i1 +1; i2 < expenses.Length; i2++)
                {
                    x2 = expenses[i2];
                    if (x1 + x2 == 2020) {
                        Console.WriteLine($"Expenses are {x1}({i1}), {x2}({i2}) => {x1*x2}");
                        break;
                    }
                }
            }
        }
        public static void Task2()
        {
            var expenses = File.ReadAllLines("data/1-expenses.txt").Select(x => int.Parse(x)).ToArray();
            int x1 = 0, x2 = 0, x3=0;
            for (int i1 = 0; i1 < expenses.Length; i1++)
            {
                x1 = expenses[i1];
                for (int i2 = i1 + 1; i2 < expenses.Length; i2++)
                {
                    x2 = expenses[i2];
                    for (int i3 = i2+1; i3 < expenses.Length; i3++)
                    {
                        x3 = expenses[i3];
                        if (x1 + x2 + x3 == 2020)
                        {
                            Console.WriteLine($"Expenses are {x1}({i1}), {x2}({i2}), {x3}({i3})  => {x1 * x2 * x3}");
                            break;
                        }
                    }
                }
            }
        }
    }
}
