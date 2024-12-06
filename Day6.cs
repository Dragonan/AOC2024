using System;
using System.Diagnostics;
using System.Drawing;

namespace AOC2024
{
    public class Day6
    {
        private static string[] Map;

        public static void Solve()
        {
            Map = PuzzleInput.Input;
            var guardDir = Directions.Up;
            var guardPos = GetPositionOf('^');
            var obstacles = GetAllPositionsOf('#');

            SolvePart2(guardPos, guardDir, obstacles);
        }

        private static void SolvePart1(Point guardPos, Directions guardDir, List<Point> obstacles)
        {
            var visitedLocations = GetGuardRoute(guardPos, guardDir, obstacles);

            Console.WriteLine(visitedLocations.Count());
        }

        private static void SolvePart2(Point guardPos, Directions guardDir, List<Point> obstacles)
        {
            var successfulBlocks = 0;
            var visitedLocations = GetGuardRoute(guardPos, guardDir, obstacles);

            foreach (var loc in visitedLocations)
                if (loc != guardPos && GetGuardRoute(guardPos, guardDir, new List<Point>(obstacles) { loc }).Count == 0)
                    successfulBlocks++;

            Console.WriteLine(successfulBlocks);
        }

        private static Point GetPositionOf(char symbol)
        {
            for (int i = 0; i < Map.Length; i++)
            {
                var match = Map[i].IndexOf(symbol);
                if (match != -1)
                    return new Point(match, i);
            }

            return new Point(-1, -1);
        }

        private static List<Point> GetAllPositionsOf(char symbol)
        {
            var result = new List<Point>();

            for (int i = 0; i < Map.Length; i++)
            {
                for (int j = 0; j < Map[i].Length; j++)
                {
                    if (Map[i][j] == symbol)
                        result.Add(new (j,i));
                }
            }

            return result;
        }

        private static List<Point> GetGuardRoute(Point guardPos, Directions guardDir, List<Point> obstacles)
        {
            var visitedLocations = new List<Point> { guardPos };
            var turningPoints = new List<(Point point, Directions dir)>();

            while(!turningPoints.Contains((guardPos, guardDir)))
            {
                turningPoints.Add((guardPos, guardDir));
                var lastStep = Point.Empty;
                var isOut = false;
                switch (guardDir)
                {
                    case Directions.Up:
                        lastStep = obstacles.Where(o => o.X == guardPos.X && o.Y < guardPos.Y).OrderByDescending(o => o.Y).FirstOrDefault();
                        isOut = lastStep.IsEmpty;
                        if (isOut)
                            lastStep = new (guardPos.X, -1);
                        lastStep.Y++;
                        break;
                    case Directions.Right:
                        lastStep = obstacles.Where(o => o.Y == guardPos.Y && o.X > guardPos.X).OrderBy(o => o.X).FirstOrDefault();
                        isOut = lastStep.IsEmpty;
                        if (isOut)
                            lastStep = new (Map[0].Length, guardPos.Y);
                        lastStep.X--;
                        break;
                    case Directions.Down:
                        lastStep = obstacles.Where(o => o.X == guardPos.X && o.Y > guardPos.Y).OrderBy(o => o.Y).FirstOrDefault();
                        isOut = lastStep.IsEmpty;
                        if (isOut)
                            lastStep = new (guardPos.X, Map.Length);
                        lastStep.Y--;
                        break;
                    case Directions.Left:
                        lastStep = obstacles.Where(o => o.Y == guardPos.Y && o.X < guardPos.X).OrderByDescending(o => o.X).FirstOrDefault();
                        isOut = lastStep.IsEmpty;
                        if (isOut)
                            lastStep = new (-1, guardPos.Y);
                        lastStep.X++;
                        break;
                    default:
                        break;
                }

                visitedLocations.AddRange(GetSteps(guardPos, lastStep));
                if (isOut)
                    return visitedLocations.Distinct().ToList();;
                guardPos = lastStep;
                guardDir = guardDir == Directions.Left ? Directions.Up : (guardDir + 1);
                
            }

            return new List<Point>();
        }

        private static List<Point> GetSteps(Point start, Point end)
        {
            int startX = Math.Min(start.X, end.X), endX = Math.Max(start.X, end.X),
                startY = Math.Min(start.Y, end.Y), endY = Math.Max(start.Y, end.Y);

            var result = new List<Point>();

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    result.Add(new (i, j));
                }
            }

            return result;
        }
    }
}
