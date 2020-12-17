using System;
using System.IO;

namespace aoc2k20
{
    class Day17
    {
        private static readonly string[] Data = File.ReadAllLines("data/17-cubes.txt");
        public static void Task1()
        {
            int tileX = Data[0].Length, tileY = Data.Length, iter = 6;
            int spaceX = tileX + 2 * iter, spaceY = tileY + 2 * iter, spaceZ = 2 * iter + 1;
            int tileX0 = (spaceX - tileX) / 2, tileY0 = (spaceY - tileY) / 2, tileZ0 = iter;
            bool[,,] space = new bool[spaceX, spaceY, spaceZ];
            for (int y = 0; y < tileY; y++)
            {
                for (int x = 0; x < tileX; x++)
                {
                    space[tileX0 + x, tileY0 + y, tileZ0] = Data[y][x] == '#';
                }
            }
            Dump(space);
            var result = 0;
            for (int i = 0; i < iter; i++)
            {
                bool[,,] space2 = Iter(space, out result);
                space = space2;
                Console.WriteLine($"ITER {i + 1}");
                Dump(space);
            }

            Console.WriteLine($"Task #1 result: {result}");
        }

        private static void Dump(bool[,,] space)
        {
            for (int z = 0; z < space.GetLength(2); z++)
            {
                for (int y = 0; y < space.GetLength(1); y++)
                {
                    for (int x = 0; x < space.GetLength(0); x++)
                    {
                        Console.Write(space[x, y, z] ? '#' : '.');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"Z={z}");
            }
        }

        private static bool[,,] Iter(bool[,,] space, out int count)
        {
            var c = 0;
            bool[,,] space2 = new bool[space.GetLength(0), space.GetLength(1), space.GetLength(2)];
            for (int x = 0; x < space.GetLength(0); x++)
            {
                for (int y = 0; y < space.GetLength(1); y++)
                {
                    for (int z = 0; z < space.GetLength(2); z++)
                    {
                        var nb = Neighbors(space, x, y, z);
                        if (space[x, y, z])
                        {
                            space2[x, y, z] = nb > 1 && nb < 4;
                        }
                        else
                        {
                            space2[x, y, z] = nb == 3;
                        }
                        c += space2[x, y, z] ? 1 : 0;
                    }
                }
            }
            count = c;
            return space2;
        }

        private static bool[,,,] Iter(bool[,,,] space, out int count)
        {
            var c = 0;
            bool[,,,] space2 = new bool[space.GetLength(0), space.GetLength(1), space.GetLength(2), space.GetLength(3)];
            for (int x = 0; x < space.GetLength(0); x++)
            {
                for (int y = 0; y < space.GetLength(1); y++)
                {
                    for (int z = 0; z < space.GetLength(2); z++)
                    {
                        for (int w = 0; w < space.GetLength(3); w++)
                        {
                            var nb = Neighbors(space, x, y, z, w);
                            if (space[x, y, z, w])
                            {
                                space2[x, y, z, w] = nb > 1 && nb < 4;
                            }
                            else
                            {
                                space2[x, y, z, w] = nb == 3;
                            }
                            c += space2[x, y, z, w] ? 1 : 0;
                        }
                    }
                }
            }
            count = c;
            return space2;
        }

        private static int Neighbors(bool[,,] space, int x, int y, int z)
        {
            int neighbors = 0;
            for (int xo = -1; xo < 2; xo++)
            {
                for (int yo = -1; yo < 2; yo++)
                {
                    for (int zo = -1; zo < 2; zo++)
                    {
                        if (x + xo < 0 || x + xo >= space.GetLength(0) ||
                            y + yo < 0 || y + yo >= space.GetLength(1) ||
                            z + zo < 0 || z + zo >= space.GetLength(2)) continue;
                        if (xo == 0 && yo == 0 && zo == 0) continue; // skip yourself
                        if (space[x + xo, y + yo, z + zo]) neighbors++;
                    }
                }
            }
            return neighbors;
        }

        private static int Neighbors(bool[,,,] space, int x, int y, int z, int w)
        {
            int neighbors = 0;
            for (int xo = -1; xo < 2; xo++)
            {
                for (int yo = -1; yo < 2; yo++)
                {
                    for (int zo = -1; zo < 2; zo++)
                    {
                        for (int wo = -1; wo < 2; wo++)
                        {
                            if (x + xo < 0 || x + xo >= space.GetLength(0) ||
                            y + yo < 0 || y + yo >= space.GetLength(1) ||
                            z + zo < 0 || z + zo >= space.GetLength(2) ||
                            w + wo < 0 || w + wo >= space.GetLength(3)) continue;
                            if (xo == 0 && yo == 0 && zo == 0 && wo == 0) continue; // skip yourself
                            if (space[x + xo, y + yo, z + zo, w + wo]) neighbors++;
                        }
                    }
                }
            }
            return neighbors;
        }

        public static void Task2()
        {
            int tileX = Data[0].Length, tileY = Data.Length, iter = 6;
            int spaceX = tileX + 2 * iter, spaceY = tileY + 2 * iter, spaceZ = 2 * iter + 1, spaceW = spaceZ;
            int tileX0 = (spaceX - tileX) / 2, tileY0 = (spaceY - tileY) / 2, tileZ0 = iter, tileW0 = iter;
            bool[,,,] space = new bool[spaceX, spaceY, spaceZ, spaceW];
            for (int y = 0; y < tileY; y++)
            {
                for (int x = 0; x < tileX; x++)
                {
                    space[tileX0 + x, tileY0 + y, tileZ0, tileW0] = Data[y][x] == '#';
                }
            }

            var result = 0;
            for (int i = 0; i < iter; i++)
            {
                bool[,,,] space2 = Iter(space, out result);
                space = space2;
            }

            Console.WriteLine($"Task #2 result: {result}");
        }
    }
}
