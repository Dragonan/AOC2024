using System;
using System.Diagnostics;

namespace AOC2024
{
    public class Day11
    {
        private static Dictionary<ulong, List<ulong>> FiveCache;
        private static Dictionary<ulong, List<ulong>> TwentyFiveCache;

        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var stones = GetStones();
            for (int i = 0; i < 25; i++)
            {
                stones = Blink(stones);
            }
            Console.WriteLine(stones.Count);
        }

        private static void SolvePart2()
        {
            FiveCache = new Dictionary<ulong, List<ulong>>();
            TwentyFiveCache = new Dictionary<ulong, List<ulong>>();
            var stones = GetStones();
            var sum = 0L;

            // foreach (var stone in stones)
            // {
            //     sum += Cycle(stone, 25);
            // }

            foreach (var stone in stones)
            {
                var subStones = new List<ulong> { stone };
                for (int i = 0; i < 3; i++)
                {
                    var newStones = new List<ulong>();
                    foreach (var subStone in subStones)
                    {
                        if (!TwentyFiveCache.ContainsKey(subStone))
                            TwentyFiveCache[subStone] = BlinkTwentyFiveTimes(subStone);

                        if (i < 2)
                            newStones.AddRange(TwentyFiveCache[subStone]);
                        else sum += TwentyFiveCache[subStone].Count;
                    }
                    subStones = newStones;
                }
            }

            

            Console.WriteLine("5 cache size: " + FiveCache.Count);
            Console.WriteLine("25 cache size: " + TwentyFiveCache.Count);
            Console.WriteLine(sum);
        }

        private static List<ulong> GetStones()
        {
            return PuzzleInput.Text.Split(' ').Select(ulong.Parse).ToList();
        }

        private static List<ulong> Blink(List <ulong> stones)
        {
            for (int i = 0; i < stones.Count; i++)
            {
                var stone = stones[i];

                if (stone == 0)
                {
                    stones[i] = 1;
                    continue;
                }

                var digits = stone.Length();
                if (digits % 2 == 0)
                {
                    var divisor = (ulong)Math.Pow(10, digits / 2);
                    stones[i] = stone / divisor;
                    stones.Insert(i+1, stone % divisor);
                    i++;
                    continue;
                }

                stones[i] *= 2024;
            }

            return stones;
        }

        private static List<ulong> BlinkFiveTimes(ulong stone)
        {
            var stones = new List<ulong> { stone };
            for (int i = 0; i < 5; i++)
            {
                stones = Blink(stones);
            }
            return stones;
        }

        private static List<ulong> BlinkTwentyFiveTimes(ulong startingStone)
        {
            var stones = new List<ulong> { startingStone };
            for (int i = 0; i < 5; i++)
            {
                var newStones = new List<ulong>();
                foreach (var stone in stones)
                {
                    if (!FiveCache.ContainsKey(stone))
                        FiveCache[stone] = BlinkFiveTimes(stone);

                    newStones.AddRange(FiveCache[stone]);
                }
                stones = newStones;
            }
            return stones;
        }
    }
}
