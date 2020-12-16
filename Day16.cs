using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2k20
{
    class Day16
    {
        private static readonly Regex Definition = new Regex(@"(?<label>[^:]+):\s(?<r1min>\d+)-(?<r1max>\d+)\sor\s(?<r2min>\d+)-(?<r2max>\d+)");

        private class FieldDefinition
        {
            public string Label { get; set; }
            public (int from, int to) R1 { get; set; }
            public (int from, int to) R2 { get; set; }
            private bool Inside(int value, (int from, int to) range) => value >= range.from && value <= range.to;
            public bool ValidField(int number) => Inside(number, R1) || Inside(number, R2);
        }

        private class TicketFieldCandidate
        {
            public int FieldIndex { get; set; }
            public ICollection<FieldDefinition> Definitions { get; set; } = new List<FieldDefinition>();
        }

        public static void Task1()
        {
            ParseTicketData(File.ReadAllLines("data/16-tickets.txt"), out List<FieldDefinition> defs, out List<int[]> tickets);

            var errCount = 0;
            foreach (var ticket in tickets)
            {
                for (int i = 0; i < ticket.Length; i++)
                {
                    var f = ticket[i];
                    var match = false;
                    foreach (var def in defs)
                    {
                        if (def.ValidField(f))
                        {
                            match = true;
                            break;
                        }
                    }
                    if (!match)
                        errCount += f;
                }
            }

            Console.WriteLine($"Task #1 result: {errCount}");
        }

        private static void ParseTicketData(string[] data,  out List<FieldDefinition> defs, out List<int[]> tickets)
        {
            int dI = 0;
            defs = ParseDefinitions(data, ref dI);
            dI += 2; // Skip blank + next label
            tickets = new List<int[]>();
            tickets.Add(ParseTicket(data[dI])); // your ticket
            dI += 3; // Skip ticket + blank + next label
            while (dI < data.Length)
            {
                tickets.Add(ParseTicket(data[dI]));
                dI++;
            }
        }
        private static int[] ParseTicket(string data) => data.Split(',').Select(int.Parse).ToArray();

        private static List<FieldDefinition> ParseDefinitions(string[] data, ref int dI)
        {
            var defs = new List<FieldDefinition>();
            while (!string.IsNullOrWhiteSpace(data[dI]))
            {
                var match = Definition.Match(data[dI]);
                defs.Add(new FieldDefinition
                {
                    Label = match.Groups["label"].Value,
                    R1 = (int.Parse(match.Groups["r1min"].Value), int.Parse(match.Groups["r1max"].Value)),
                    R2 = (int.Parse(match.Groups["r2min"].Value), int.Parse(match.Groups["r2max"].Value))
                }); ;
                dI++;
            }

            return defs;
        }

        public static void Task2()
        {
            ParseTicketData(File.ReadAllLines("data/16-tickets.txt"), out List<FieldDefinition> defs, out List<int[]> tickets);

            // Discard invalid tickets
            //
            tickets = tickets.Where(t => ValidateTicket(defs, t)).ToList();

            // Extract all possible field candidates
            //
            var extracted = new List<TicketFieldCandidate>();
            for (int fIdx = 0; fIdx < defs.Count; fIdx++) // Also # of fields in tickets
            {
                var cand = new TicketFieldCandidate() { FieldIndex = fIdx };
                extracted.Add(cand);
                var fValues = tickets.Select(t => t[fIdx]).ToArray();
                for (int dIdx = 0; dIdx < defs.Count; dIdx++)
                {
                    var def = defs[dIdx];
                    if (fValues.All(f => def.ValidField(f)))
                    {
                        cand.Definitions.Add(def);
                    }
                }
            }

            // Process candidates in order, removing obvious candidates (single definition match)
            //
            var processed = new List<TicketFieldCandidate>();
            while (extracted.Any())
            {
                var dc0 = extracted.FirstOrDefault(dc => dc.Definitions.Count == 1);
                extracted.Remove(dc0);
                var d0 = dc0.Definitions.First();
                foreach (var dc in extracted)
                {
                    dc.Definitions = dc.Definitions.Where(d => d != d0).ToList(); // Definition not applicable to other fields
                }
                processed.Add(dc0);
            }

            // Calculate result
            //
            long fProduct = 1;
            foreach (var item in processed
                .OrderBy(p => p.FieldIndex)
                .Where(p => p.Definitions.First().Label.StartsWith("departure"))
                .Select(p => p.FieldIndex))
            {
                fProduct *= tickets[0][item];
            }

            Console.WriteLine($"Task #2 result: {fProduct}");
        }

        private static bool ValidateTicket(List<FieldDefinition> defs, int[] ticket)
        {
            for (int i = 0; i < ticket.Length; i++)
            {
                var f = ticket[i];
                var match = false;
                foreach (var def in defs)
                {
                    if (def.ValidField(f))
                    {
                        match = true;
                        break;
                    }
                }
                if (!match)
                    return false;
            }
            return true;
        }
    }
}
