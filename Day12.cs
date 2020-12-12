using System;
using System.IO;

namespace aoc2k20
{
    class Day12
    {
        private static readonly string[] Data = File.ReadAllLines("data/12-navigation.txt");

        private static int DirEW(int dir)
        {
            return (dir % 360) switch
            {
                90 => 1, // East
                270 => -1, // West
                _ => 0 // N/S
            };
        }
        private static int DirNS(int dir)
        {
            return (dir % 360) switch
            {
                0 => 1, // North
                180 => -1, // South
                _ => 0 // E/W
            };
        }

        private static void Move(string instr, Position p)
        {
            var val = int.Parse(instr.Substring(1));
            switch (instr[0])
            {
                case 'N': p.ns += val;
                    break;
                case 'E': p.ew += val;
                    break;
                case 'S': p.ns -= val;
                    break;
                case 'W': p.ew -= val;
                    break;
                case 'L': p.dir = (p.dir + 360 - val) % 360;
                    break;
                case 'R': p.dir = (p.dir + val) % 360;
                    break;
                case 'F':
                    p.ew += DirEW(p.dir) * val;
                    p.ns += DirNS(p.dir) * val;
                    break;
            }
            Console.WriteLine($"Move {instr.PadRight(5)}: D={p.dir:D3} NS={p.ns:D4}, EW={p.ew:D5}");
        }

        private static void Swizzle(int dRot, ref int ns, ref int ew)
        {
            int tmp = ew;
            switch (dRot)
            {
                case 90: // R90
                case -270: // L270
                    ew = ns; // 90 ew = ns, ns = -ew
                    ns = -tmp;
                    break;
                case 180: // R180
                case -180: // L180
                    ew = -ew; // 180 ew = -ew, ns = -ns
                    ns = -ns;
                    break;
                case 270: // R270
                case -90: // L90
                    ew = -ns; // 270 ew = -ns, ns = ew
                    ns = tmp;
                    break;
            }
        }

        private static void MoveAround(string instr, Position wp, Position ship)
        {
            var val = int.Parse(instr.Substring(1));
            switch (instr[0])
            {
                case 'N': wp.ns += val;
                    break;
                case 'E': wp.ew += val;
                    break;
                case 'S': wp.ns -= val;
                    break;
                case 'W': wp.ew -= val;
                    break;
                case 'L': Swizzle(-val, ref wp.ns, ref wp.ew);
                    break;
                case 'R': Swizzle(val, ref wp.ns, ref wp.ew);
                    break;
                case 'F':
                    ship.ns += wp.ns * val;
                    ship.ew += wp.ew * val;
                    break;
            }
            Console.WriteLine($"Move {instr.PadRight(5)}: Ship[NS={ship.ns:D4}, EW={ship.ew:D4}] WP[NS={wp.ns:D4}, EW={wp.ew:D4}]");
        }


        public static void Task1()
        {
            Position ship = new Position() {dir = 90}; // Start facing east
            foreach (var instr in Data)
            {
                Move(instr, ship);
            }
            var result = ship.ManDist;
            Console.WriteLine($"Task #1 result: {result}");
        }
        public static void Task2()
        {
            Position ship = new Position(), wp = new Position() {ns = 1, ew = 10};
            foreach (var instr in Data)
            {
                MoveAround(instr, wp, ship);
            }
            var result = ship.ManDist;
            Console.WriteLine($"Task #2 result: {result}");
        }
    }

    public class Position
    {
        public int dir;
        public int ew;
        public int ns;

        public int ManDist => Math.Abs(ns) + Math.Abs(ew);
    }
}
