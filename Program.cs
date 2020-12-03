﻿using System;

namespace aoc2k20
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(int.Parse(args[0]), int.Parse(args[1]));
        }

        private Action[][] tasks = new[]{
            new Action[] {Day1.Task1, Day1.Task2 },
            new Action[] {Day2.Task1, Day2.Task2 },
            new Action[] {Day3.Task1, Day3.Task2 },
            new Action[] {DayX.Task1, DayX.Task2 }
        };

        public void Run(int day, int task)
        {
            tasks[day-1][task-1].Invoke();
        }
    }
}