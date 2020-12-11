using System;
using System.IO;
using System.Text;

namespace aoc2k20
{
    class Map
    {
        private readonly char outer;
        private readonly char[,] map;
        public int Width => map.GetLength(0);
        public  int Height => map.GetLength(1);
        public Map(string[] data, char outer)
        {
            this.outer = outer;
            map = new char[data[0].Length, data.Length];
            for (int y = 0; y < data.Length; y++)
            {
                var l = data[y];
                for (int x = 0; x < l.Length; x++)
                {
                    map[x, y] = l[x];
                }
            }
        }

        private Map(char[,] map, char outer)
        {
            this.map = map;
            this.outer = outer;
        }

        public char At(int x, int y) => (y < 0 || x < 0 || y >= Height || x >= Width) ? outer : map[x, y];
        public void Set(int x, int y, char c)
        {
            if (y < 0 || x < 0 || y >= map.Length || x >= Width) return;
            map[x, y] = c;
        }

        public int Count(char c)
        {
            var count = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (map[x,y] == c) count++;
                }
            }
            return count;
        }

        public Map Clone()
        {
            var m2 = new char[Width, Height];
            Array.Copy(map, m2, Width * Height);
            return new Map(m2, outer);
        }

        public override bool Equals(object? obj)
        {
            var mo = obj as Map;
            if (mo == null || mo.outer != outer || mo.Width != Width || mo.Height != Height) return false;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (map[x,y] != mo.map[x,y]) return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(capacity: Width*Height+Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    sb.Append(map[x,y]);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    class Day11
    {
        private static readonly string[] Data = File.ReadAllLines("data/11-seats.txt");
        
        private static bool Free(Map m, int x, int y) => m.At(x,y) != OccSeat;
        private const char FreeSeat = 'L';
        private const char OccSeat = '#';
        private const char Floor = '.';

        private static int OccNeighbors(Map m, int x, int y)
        {
            var n = 0;
            if (!Free(m,x - 1, y - 1)) n++;
            if (!Free(m, x, y - 1)) n++;
            if (!Free(m, x + 1, y - 1)) n++;
            if (!Free(m, x - 1, y)) n++;
            if (!Free(m, x + 1, y)) n++;
            if (!Free(m, x - 1, y + 1)) n++;
            if (!Free(m, x, y + 1)) n++;
            if (!Free(m, x + 1, y + 1)) n++;
            return n;
        }

        private static char Task1EvalCell(Map m, int x, int y)
        {
            var p = m.At(x, y);
            if (p == Floor) return Floor;

            var on = OccNeighbors(m, x, y);
            switch (p)
            {
                case FreeSeat when @on == 0:
                    return OccSeat;
                case OccSeat when @on >= 4:
                    return FreeSeat;
            }
            return p;
        }

        private static Map Eval(Map m0, Func<Map, int, int, char> evalCell)
        {
            var m = m0.Clone();
            for (int y = 0; y < m.Height; y++)
            {
                for (int x = 0; x < m.Width; x++)
                {
                    m.Set(x, y, evalCell(m0, x, y));
                }
            }
            return m;
        }

        private static void LogIter(Map m, int iter)
        {
            #if DUMPMAP
            Console.Clear();
            Console.WriteLine($"Iter {iter}:");
            Console.WriteLine(m);
            Console.ReadKey(true);
            #endif
        }

        public static void Task1()
        {
            var map0 = new Map(Data, Floor);
            LogIter(map0, 0);

            var map1 = Eval(map0, Task1EvalCell);
            var iter = 1;
            while (!map0.Equals(map1))
            {
                LogIter(map1, iter);
                map0 = map1;
                map1 = Eval(map0, Task1EvalCell);
                iter++;
            }

            var result = map0.Count(OccSeat);
            Console.WriteLine($"Task #1 result: {result} in {iter} iterations");
        }

        private static int ShootRay(Map m, int x0, int y0, int xd, int yd)
        {
            int x = x0 + xd, y = y0 + yd;
            var c = m.At(x, y);
            if (c == FreeSeat || c == OccSeat) return c == OccSeat ? 1 : 0;
            return ShootRay(m, x, y, xd, yd);
        }

        private static int OccLineOfSight(Map m, int x, int y)
        {
            return
                ShootRay(m, x, y, -1, -1) +
                ShootRay(m, x, y, 0, -1) +
                ShootRay(m, x, y, 1, -1) +
                ShootRay(m, x, y, -1, 0) +
                ShootRay(m, x, y, 1, 0) +
                ShootRay(m, x, y, -1, 1) +
                ShootRay(m, x, y, 0, 1) +
                ShootRay(m, x, y, 1, 1);
        }

        private static char Task2EvalCell(Map m, int x, int y)
        {
            var p = m.At(x, y);
            if (p == Floor) return Floor;

            var on = OccLineOfSight(m, x, y);
            switch (p)
            {
                case FreeSeat when @on == 0:
                    return OccSeat;
                case OccSeat when @on >= 5:
                    return FreeSeat;
            }
            return p;
        }

        public static void Task2()
        {
            var map0 = new Map(Data, FreeSeat);
            LogIter(map0, 0);

            var map1 = Eval(map0, Task2EvalCell);
            var iter = 1;
            while (!map0.Equals(map1))
            {
                LogIter(map1, iter);
                map0 = map1;
                map1 = Eval(map0, Task2EvalCell);
                iter++;
            }

            var result = map0.Count(OccSeat);
            Console.WriteLine($"Task #1 result: {result} in {iter} iterations");
        }
    }
}
