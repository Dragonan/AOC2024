using System;
using System.Drawing;

namespace AOC2024
{
    public class Day8
    {
        private static string[] Map;
        public static void Solve()
        {
            Map = PuzzleInput.Input;
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var frequencies = GetFrequencies();
            var bounds = new Point(Map[0].Length, Map.Length);
            var antinodes = new List<Point>();

            foreach (var frequency in frequencies)
            {
                var antennas = frequency.Value;
                for (int i = 0; i < antennas.Count - 1; i++)
                {
                    for (int j = i + 1; j < antennas.Count; j++)
                    {
                        var a1 = antennas[i];
                        var a2 = antennas[j];
                        var diff = new Point(a1.X - a2.X, a1.Y - a2.Y);

                        var antinode1 = new Point(a1.X + diff.X, a1.Y + diff.Y);
                        var antinode2 = new Point(a2.X - diff.X, a2.Y - diff.Y);

                        if (!antinode1.IsOutOfBounds(bounds.X, bounds.Y))
                            antinodes.Add(antinode1);
                        
                        if (!antinode2.IsOutOfBounds(bounds.X, bounds.Y))
                            antinodes.Add(antinode2);
                    }
                }
            }

            Console.WriteLine(antinodes.Distinct().Count());
        }

        private static void SolvePart2()
        {
            var frequencies = GetFrequencies();
            var bounds = new Point(Map[0].Length, Map.Length);
            var antinodes = new List<Point>();

            foreach (var frequency in frequencies)
            {
                var antennas = frequency.Value;
                for (int i = 0; i < antennas.Count - 1; i++)
                {
                    for (int j = i + 1; j < antennas.Count; j++)
                    {
                        var a1 = antennas[i];
                        var a2 = antennas[j];
                        var diff = new Point(a1.X - a2.X, a1.Y - a2.Y);

                        while (!a1.IsOutOfBounds(bounds.X, bounds.Y))
                        {
                            antinodes.Add(a1);
                            a1 = new Point(a1.X + diff.X, a1.Y + diff.Y);
                        }

                        while (!a2.IsOutOfBounds(bounds.X, bounds.Y))
                        {
                            antinodes.Add(a2);
                            a2 = new Point(a2.X - diff.X, a2.Y - diff.Y);
                        }
                    }
                }
            }

            Console.WriteLine(antinodes.Distinct().Count());
        }

        private static Dictionary<char, List<Point>> GetFrequencies()
        {
            var result = new Dictionary<char, List<Point>>();

            for (int i = 0; i < Map.Length; i++)
            {
                for (int j = 0; j < Map[i].Length; j++)
                {
                    if (Map[i][j] != '.')
                    {
                        if (!result.ContainsKey(Map[i][j]))
                            result.Add(Map[i][j], new List<Point>());
                        result[Map[i][j]].Add(new (j, i));
                    }
                }
            }

            return result;
        }
    }
}
