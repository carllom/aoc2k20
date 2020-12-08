using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day8
    {
        private static readonly Ins[] Data = File.ReadAllLines("data/8-bootcode.txt").Select(l => new Ins(l)).ToArray();
        public static void Task1()
        {
            var vm = new Vm();
            vm.Execute(Data);
            Console.WriteLine($"Task #1 result: {vm.Acc}");
        }
        public static void Task2()
        {
            var vm = new Vm();
            
            var swIns = 0;

            while (swIns < Data.Length)
            {
                var si = Data[swIns++];
                if (si.Mnemonic == "acc") continue;
                si.Mnemonic = si.Mnemonic == "jmp" ? "nop" : "jmp"; // Swap
                vm.Execute(Data);
                if (vm.Pc == Data.Length) break;
                si.Mnemonic = si.Mnemonic == "jmp" ? "nop" : "jmp"; // Restore
                foreach (var ins in Data) { ins.Count = 0; } // Reset instruction count
            }
            Console.WriteLine($"Task #2 result: {vm.Acc}");
        }

        private class Vm
        {
            public long Acc { get; set; }
            public int Pc { get; set; }

            public void Execute(Ins[] program)
            {
                Reset();
                var ci = program[Pc];
                while (Pc < program.Length)
                {
                    if (ci.Count > 0) break; // Inf loop
                    switch (ci.Mnemonic)
                    {
                        case "acc":
                            Acc += ci.Op;
                            Pc++;
                            break;
                        case "jmp":
                            Pc += ci.Op;
                            break;
                        case "nop":
                            Pc++;
                            break;
                    }
                    ci.Count++;
                    if (Pc >= program.Length) break;
                    ci = program[Pc];
                }
            }

            public void Reset()
            {
                Pc = 0;
                Acc = 0;
            }
        }

        private class Ins
        {
            public string Mnemonic { get; set; }
            public int Op { get; }
            public int Count { get; set; }

            public Ins(string line)
            {
                var tok = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Mnemonic = tok[0];
                if (tok.Length > 1) Op = int.Parse(tok[1], NumberStyles.AllowLeadingSign);
            }
        }
    }
}
