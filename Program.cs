using System;

namespace aoc2k20
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(int.Parse(args[0]), int.Parse(args[1]));
        }

        private readonly Action[][] tasks = {
            new Action[] {Day1.Task1, Day1.Task2 },
            new Action[] {Day2.Task1, Day2.Task2 },
            new Action[] {Day3.Task1, Day3.Task2 },
            new Action[] {Day4.Task1, Day4.Task2 },
            new Action[] {Day5.Task1, Day5.Task2 },
            new Action[] {Day6.Task1, Day6.Task2 },
            new Action[] {Day7.Task1, Day7.Task2 },
            new Action[] {Day8.Task1, Day8.Task2 },
            new Action[] {Day9.Task1, Day9.Task2 },
            new Action[] {Day10.Task1, Day10.Task2 },
            new Action[] {Day11.Task1, Day11.Task2 },
            new Action[] {Day12.Task1, Day12.Task2 },
            new Action[] {Day13.Task1, Day13.Task2 },
            new Action[] {Day14.Task1, Day14.Task2 },
            new Action[] {Day15.Task1, Day15.Task2 },
            new Action[] {Day16.Task1, Day16.Task2 },
            new Action[] {Day17.Task1, Day17.Task2 },
        };

        public void Run(int day, int task)
        {
            tasks[day-1][task-1].Invoke();
        }
    }
}
