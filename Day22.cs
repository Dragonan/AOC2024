using System;
using System.Diagnostics.CodeAnalysis;

namespace AOC2024
{
    public class Day22
    {
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var numbers = GetNumbers();
            var sum = 0L;
            foreach (var number in numbers)
            {
                var n = number;
                for (int i = 0; i < 2000; i++)
                {
                    n = Evolve(n);
                }
                sum += n;
            }
            Console.WriteLine(sum);
        }

        private static void SolvePart2()
        {
            var sellers = GetNumbers();
            var sellerPrices = new List<int[]>();
            var sellerPriceChanges = new List<int[]>();
            foreach (var seller in sellers)
            {
                var prices = new int[2001];
                var priceChanges = new int[2001];
                prices[0] = seller % 10;
                var n = seller;
                for (int i = 0; i < 2000; i++)
                {
                    n = Evolve(n);
                    prices[i+1] = n % 10;
                    priceChanges[i+1] = prices[i+1] - prices[i];
                }

                sellerPrices.Add(prices);
                sellerPriceChanges.Add(priceChanges);
            }

            var priceCache = new int?[19,19,19,19,sellers.Length];
            for (int s = 0; s < sellerPriceChanges.Count; s++)
            {
                var ch = sellerPriceChanges[s];
                for (int i = 4; i < ch.Length; i++)
                {
                    if (priceCache[ch[i-3]+9, ch[i-2]+9, ch[i-1]+9, ch[i]+9, s] == null)
                        priceCache[ch[i-3]+9, ch[i-2]+9, ch[i-1]+9, ch[i]+9, s] = sellerPrices[s][i];
                }
            }

            var maxBananas = 0;
            for (int a = -9; a < 10; a++)
            {
                var bMin = Math.Max(-9,-9-a);
                var bMax = Math.Min(10,10-a);
                for (int b = bMin; b < bMax; b++)
                {
                    var cMin = Math.Max(-9,-9-a-b);
                    var cMax = Math.Min(10,10-a-b);
                    for (int c = cMin; c < cMax; c++)
                    {
                        var dMin = Math.Max(-9,-9-a-b-c);
                        var dMax = Math.Min(10,10-a-b-c);
                        for (int d = dMin; d < dMax; d++)
                        {
                            var bananas = 0;
                            for (int s = 0; s < sellers.Length; s++)
                                bananas += priceCache[a+9,b+9,c+9,d+9,s] ?? 0;
                            maxBananas = Math.Max(maxBananas, bananas);
                        }
                    }
                }
            }
            
            Console.WriteLine(maxBananas);
        }

        private static long Mix (int number, long value) => number ^ value;
        private static int Prune (long number) => (int)(number % 16777216);

        private static int Evolve(int number)
        {
            number = Prune(Mix(number, number*64L));
            number = Prune(Mix(number, (long)Math.Truncate(number/32d)));
            return Prune(Mix(number, number*2048L));
        }

        private static int[] GetNumbers() => PuzzleInput.Input.Select(int.Parse).ToArray();
    }
}
