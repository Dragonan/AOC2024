using System;
using System.Drawing;

namespace AOC2024
{
    public class Day12
    {
        private static string[] Garden;
        private static bool[][] CheckedPlots;

        public static void Solve()
        {
            Garden = PuzzleInput.Input;
            CheckedPlots = Garden.Select(g => Enumerable.Repeat(false, g.Length).ToArray()).ToArray();

            SolvePart2();
        }

        private static void SolvePart1()
        {
            var sum = 0;

            for (int y = 0; y < Garden.Length; y++)
            {
                for (int x = 0; x < Garden[y].Length; x++)
                {
                    if (CheckedPlots[y][x])
                        continue;
                    
                    sum += GetAreaAndPermiter(Garden[y][x], new (x,y));
                }
            }

            Console.WriteLine(sum);
        }

        private static void SolvePart2()
        {
            var sum = 0;

            for (int y = 0; y < Garden.Length; y++)
            {
                for (int x = 0; x < Garden[y].Length; x++)
                {
                    if (CheckedPlots[y][x])
                        continue;
                    
                    sum += GetAreaAndPermiter(Garden[y][x], new (x,y), true);
                }
            }

            Console.WriteLine(sum);
        }

        private static int GetAreaAndPermiter(char letter, Point firstPlot, bool withFence = false)
        {
            var toCheck = new List<Point> { firstPlot };
            var area = 0;
            var perimeter = 0;
            var fences = new List<(Point Plot, Directions Side)>();

            while (toCheck.Any())
            {
                var nextToCheck = new List<Point>();
                foreach (var plot in toCheck)
                {
                    area++;
                    CheckedPlots[plot.Y][plot.X] = true;


                    foreach (var dir in (Directions[])Enum.GetValues(typeof(Directions)))
                    {
                        var nb = plot.Move(dir);
                        if (nb.IsOutOfBounds(Garden[0].Length, Garden.Length) || Garden[nb.Y][nb.X] != letter)
                        {
                            if (withFence)
                                fences.Add((plot, dir));
                            else
                                perimeter++;
                        }
                        else if (!CheckedPlots[nb.Y][nb.X])
                            nextToCheck.Add(nb);
                    }
                }
                toCheck = nextToCheck.Distinct().ToList();
                //DebugDraw(toCheck);
            }

            if (withFence)
            {
                var sides = 0;
                var fencesByDir = fences.GroupBy(f => f.Side);
                foreach (var group in fencesByDir)
                {
                    var horizontal = group.Key == Directions.Up || group.Key == Directions.Down;
                    var ordered = group
                        .GroupBy(f => horizontal ? f.Plot.Y : f.Plot.X)
                        .Select(g => g.Select(f => horizontal ? f.Plot.X : f.Plot.Y).Order().ToArray());
                    sides += ordered.Count();
                    
                    foreach (var sideFence in ordered)
                    {
                        for (int i = 1; i < sideFence.Count(); i++)
                        {
                            if (sideFence[i-1] + 1 != sideFence[i])
                                sides++;
                        }
                    }
                }

                return area * sides;
            }

            return area * perimeter;
        }

        private static void DebugDraw(List<Point> currentChecks)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < Garden.Length; y++)
            {
                for (int x = 0; x < Garden[y].Length; x++)
                {
                    if (CheckedPlots[y][x])
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (currentChecks.Contains(new (x,y)))
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    else
                        Console.ForegroundColor = ConsoleColor.White;
                    
                    Console.Write(Garden[y][x]);
                }
                Console.WriteLine();
            }

            Thread.Sleep(400);
        }
    }
}
