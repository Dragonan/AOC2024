using System;
using System.Text.RegularExpressions;

namespace AOC2024
{
    public class Day3
    {
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            Console.WriteLine(GetSumOfValidMuls(PuzzleInput.Text));
        }

        private static void SolvePart2()
        {
            var enabledOnly = string.Join("", PuzzleInput.Text.Split("do()").Select(d => d.Split("don't()")[0]));
            Console.WriteLine(GetSumOfValidMuls(enabledOnly));
        }

        private static int GetSumOfValidMuls(string text)
        {
            return Regex.Matches(text, @"mul\((\d+),(\d+)\)").Select(m => mul(m.Groups[1].Value, m.Groups[2].Value)).Sum();
        }

        private static int mul(string a, string b)
        {
            return int.Parse(a) * int.Parse(b);
        }
    }
}
