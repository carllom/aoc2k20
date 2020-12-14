using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace aoc2k20
{
    class Day14
    {
        private static Regex rxCmd = new Regex(@"(?<cmd>mask|mem)(\[(?<adr>\d+)\])?\s*=\s*(?<val>\w+)");
        private const string TCommand = "cmd";
        private const string TAddress = "adr";
        private const string TValue = "val";
        private const string MaskCmd = "mask";
        private const string MemCmd = "mem";
        private static readonly string[] Data = File.ReadAllLines("data/14-docking.txt");
        public static void Task1()
        {
            var insns = Data.Select(c => rxCmd.Match(c).Groups.Values.ToDictionary(g => g.Name, g => g.Value));

            ulong bVal = 0L; // Base value
            ulong mask = 0L; // 
            var mem = new Dictionary<int, ulong>();

            foreach (var ins in insns)
            {
                switch (ins[TCommand])
                {
                    case MaskCmd:
                        bVal = CreateMask(ins[TValue], c => (uint)(c == 'X' ? 0 : int.Parse(c.ToString())) );
                        mask = CreateMask(ins[TValue], c => (uint)(c == 'X' ? 1 : 0));
                        break;
                    case MemCmd:
                        var v = ulong.Parse(ins[TValue]);
                        mem[int.Parse(ins[TAddress])] = bVal | (mask & v);
                        break;
                }
            }

            var result = (ulong)mem.Values.Sum(v => (long)v);
            Console.WriteLine($"Task #1 result: {result}");
        }

        private static ulong Mask(string @in, in ulong mMask, in ulong vMask)
        {
            throw new NotImplementedException();
        }

        public static ulong CreateMask(string value, Func<char,uint> cOp)
        {
            ulong result = 0L;
            foreach (var c in value)
            {
                result <<= 1;
                result |= cOp(c);
            }
            return result;
        }

        public static void Task2()
        {
            var insns = Data.Select(c => rxCmd.Match(c).Groups.Values.ToDictionary(g => g.Name, g => g.Value));

            string mask = ""; // 
            var mem = new Dictionary<ulong, ulong>();

            foreach (var ins in insns)
            {
                switch (ins[TCommand])
                {
                    case MaskCmd:
                        mask = ins[TValue];
                        break;
                    case MemCmd:
                        var v = ulong.Parse(ins[TValue]);
                        var adr = Convert.ToString(int.Parse(ins[TAddress]), 2).PadLeft(36, '0');
                        var sb = new StringBuilder(36);
                        for (int i = 0; i < 36; i++)
                        {
                            sb.Append(mask[i] == '0' ? adr[i] : mask[i]);
                        }
                        Write(mem, sb.ToString(), v);
                        break;
                }
            }

            var result = (ulong)mem.Values.Sum(v => (long)v);
            Console.WriteLine($"Task #2 result: {result}");
        }

        public static void Write(Dictionary<ulong,ulong> mem, string address, ulong value)
        {
            var xI = address.IndexOf('X');
            if (xI < 0)
            {
                mem[Convert.ToUInt64(address, 2)] = value;
                return;
            }

            var sb = new StringBuilder(address);
            sb[xI] = '0';
            Write(mem, sb.ToString(), value);
            sb[xI] = '1';
            Write(mem, sb.ToString(), value);
        }
    }
}
