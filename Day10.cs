using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2k20
{
    class Day10
    {
        private static readonly int[] Adapters = File.ReadAllLines("data/10-adapters.txt").Select(int.Parse).OrderBy(a => a).ToArray();
        public static void Task1()
        {
            var diffs = new int[4];
            var curr = 0; // outlet
            var device = Adapters.Max() + 3;
            foreach (var i in Adapters)
            {
                var delta = i - curr;
                diffs[delta] += 1;
                curr = i;
            }

            diffs[device - curr] += 1;

            Console.WriteLine($"Task #1 result: {diffs[1]*diffs[3]}");
        }
        public static void Task2()
        {
            // Divide into chunks with distance 2 in between
            var chunks = new List<List<int>> {new List<int>()};
            var lastAdapter = 0;
            var adapters = new List<int>();
            adapters.Add(0); // outlet
            adapters.AddRange(Adapters); // adapters
            adapters.Add(Adapters.Max() + 3); // device
            foreach (var adapter in  adapters )
            {
                if (adapter - lastAdapter >= 2)
                { 
                    chunks.Add(new List<int>());
                }

                chunks.Last().Add(adapter);
                lastAdapter = adapter;
            }

            var numComb = new int[chunks.Count];
            for (int cIdx = 0; cIdx < chunks.Count; cIdx++)
            {
                numComb[cIdx] = 1; // All chunks have at least one combination - all adapters in place

                var chunk = chunks[cIdx];
                var cLen = chunk.Count;

                // Trivial case for chunks of size 1 and 2 - only one possible combination
                if (cLen < 3) continue;


                var maxPerm = (1 << (cLen - 2)) - 1; // Max number of permutations (the -1 in the end is to skip the 111..(untouched) combination

                // Chunks with size 3+ 
                for (int perm = 0; perm < maxPerm; perm++)
                {
                    // Create a permutation copy of the list
                    var cPerm = new List<int>();
                    cPerm.Add(chunk.First()); // Add start edge adapter
                    var permBits = perm;
                    for (int iBit= 0; iBit < cLen-2; iBit++)
                    {
                        if ((permBits & 1) == 1) cPerm.Add(chunk.Skip(iBit+1).First()); // Copy adapter if bit is 1
                        permBits >>= 1; // Shift down
                    }
                    cPerm.Add(chunk.Last()); // Add end edge adapter

                    // Validate
                    lastAdapter = cPerm.First();
                    var valid = true;
                    foreach (var adapter in cPerm.Skip(1)) // Skip the first adapter
                    {
                        if (adapter - lastAdapter > 3)
                        {
                            valid = false;
                            break;
                        }
                        lastAdapter = adapter;
                    }

                    if (valid) numComb[cIdx] += 1; // A valid combination
                }
            }
            
            var result = 1L;
            foreach (var comb in numComb)
            {
                result *= comb;
            }
            Console.WriteLine($"Task #2 result: {result}");
        }
    }
}
/*

000000000011111111112222
012345678901234567890123
=X  XXXX  XXX  XX  X  =    (0), 1, 4, 5, 6, 7, 10, 11, 12, 15, 16, 19, (22)
=X  XXXX  X X  XX  X  =    (0), 1, 4, 5, 6, 7, 10, 12, 15, 16, 19, (22)
=X  XX X  XXX  XX  X  =    (0), 1, 4, 5, 7, 10, 11, 12, 15, 16, 19, (22)
=X  XX X  X X  XX  X  =    (0), 1, 4, 5, 7, 10, 12, 15, 16, 19, (22)
=X  X XX  XXX  XX  X  =    (0), 1, 4, 6, 7, 10, 11, 12, 15, 16, 19, (22)
=X  X XX  X X  XX  X  =    (0), 1, 4, 6, 7, 10, 12, 15, 16, 19, (22)
=X  X  X  XXX  XX  X  =    (0), 1, 4, 7, 10, 11, 12, 15, 16, 19, (22)
=X  X  X  X X  XX  X  =    (0), 1, 4, 7, 10, 12, 15, 16, 19, (22)

So, what we should focus on is the max distance between streaks of X:es
We should "poke holes" (remove adapters) in streaks of X:es while maintaining "distance"

A streak of X:es could have holes on the edges and on the inside. Outside holes affect the allowed combinations of next streak.
Is it possible to break down into smaller cases?

Distances between streaks could be 1 or 2. 1 allows outer hole of left *or* right streak. 2 allows none.

X => .

X. => .. (must have 0d left and right)
.X => .. (must have 0d left and right)

X.X => ..X | X.. | 
XXX => ..X | .X. | .XX | X.. | X.X | XX. => 

Could we just scan it? (can we remove X@i?)
Then out of the list of possible removals - for the adjacent candidates, can we remove both?
Will there be tricky dependencies for long streaks?

The presence of X@i will only affect other X:es within range 
So, candidates for removal need only to check for other candidates within its range

1. List possible single-X removals. Best possible number of combinations is 2^#remX
2. if distance of removal to its neighbor is >3 then it cannot 

#
# Solution
#

Compromise - Partition the list in chunks with distance 2 between them.
Brute force the combinations within chunks. The total result is the product of the chunk results 

Now, the edges must always be present, that is we always have chunks like # (1), ## (2), #?# (3), #??# (4), #???# (5), #????# (6)
The unknowns(?) can be an adapter or empty
Length 1 and 2 are trivial - no adapter can be removed => 1 valid combination
For length 3+ we try every permutation of the unknowns (create permutations by iterating over numbers 0..2^(length-2)-1: 3=>0..1, 4=>0..3, 5=0..7
read as a binary number and remove an adapter if its corresponding bit is 0. If the chunk is still valid it is a combination. 

Don't forget to add the wall outlet - the first adapter could possibly be removed.
the device is distance 3, so should not contribute, but should be added for symmetry

 */