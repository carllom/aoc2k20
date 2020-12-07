using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day7
    {
        private static readonly string[] Data = File.ReadAllLines("data/7-luggage.txt");

        public static void Task1()
        {
            var bags = ParseGraph();
            var count = Ancestors(bags["shiny gold"]).Distinct().Count();
            Console.WriteLine($"Task #1 result: {count}");
        }

        private static ICollection<string> Ancestors(Node bag)
        {
            var parents = new List<string>();
            foreach (var parent in bag.Parents)
            {
                parents.Add(parent.Name);
                parents.AddRange(Ancestors(parent));
            }
            return parents;
        }

        public static void Task2()
        {
            var bags = ParseGraph();
            var result = NumBags(bags["shiny gold"]) - 1; // Remove the gold bag from the count.. d-oh!
            Console.WriteLine($"Task #2 result: {result}");
        }

        private static int NumBags(Node bag)
        {
            var count = 1; // This bag
            foreach (var child in bag.Children)
            {
                count += child.Quantity * NumBags(child.Bag);
            }
            return count;
        }

        private class Node
        {
            public string Name { get; }
            public ICollection<Ref> Children { get; } = new List<Ref>();
            public ICollection<Node> Parents { get; } = new List<Node>();
            public Node(string name)
            {
                Name = name;
            }
        }

        private class Ref
        {
            public int Quantity { get; }
            public Node Bag { get; }

            public Ref(Node bag, int quantity)
            {
                Bag = bag;
                Quantity = quantity;
            }
        }

        private static Dictionary<string,Node> ParseGraph()
        {
            var bags = new Dictionary<string, Node>();
            foreach (var rule in Data)
            {
                var r = rule.Split(new[] { " bags contain ", " bags", " bag", ", ", ".", "no other" }, StringSplitOptions.RemoveEmptyEntries);
                if (!bags.ContainsKey(r[0])) bags[r[0]] = new Node(r[0]);
                var bag = bags[r[0]];
                for (int i = 1; i < r.Length; i++)
                {
                    var ws = r[i].IndexOf(' ');
                    var qty = int.Parse(r[i].Substring(0, ws));
                    var cName = r[i].Substring(ws + 1);
                    if (!bags.ContainsKey(cName)) bags[cName] = new Node(cName);
                    var child = bags[cName];
                    bag.Children.Add(new Ref(child, qty));
                    child.Parents.Add(bag);
                }
            }
            return bags;
        }
    }
}
