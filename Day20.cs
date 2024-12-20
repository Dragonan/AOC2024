using System;
using System.Drawing;
using System.Reflection.Emit;

namespace AOC2024
{
    public class Day20
    {
        private const char WALL = '#';
        private const char FREE = '.';
        private const int MINSKIP = 100;
        private static string[] Labyrinth;
        private static List<Point> Path;
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            MapPath();
            var shortcuts = GetBestShortcuts();
            Console.WriteLine(shortcuts);
        }

        private static void SolvePart2()
        {
            MapPath();
            var shortcuts = GetBestShortcutsForPart2();
            Console.WriteLine(shortcuts);
        }

        private static void MapPath()
        {
            Labyrinth = PuzzleInput.Input;
            Path = new List<Point>();
            Point start = new Point(0,0), end = new Point(0,0);
            bool startFound = false, endFound = false;
            for (int y = 0; y < Labyrinth.Length; y++)
            {
                for (int x = 0; x < Labyrinth[y].Length; x++)
                {
                    if (Labyrinth[y][x] == 'S')
                    {
                        start = new Point(x,y);
                        startFound = true;
                    }
                    else if (Labyrinth[y][x] == 'E')
                    {
                        end = new Point(x,y);
                        endFound = true;
                    }
                    
                    if (startFound && endFound)
                        break;
                }
            }

            var current = start;
            Path.Add(start);
            Directions? from = null;
            
            while (current != end)
            {
                for (int i = 1; i <= 4; i++)
                {
                    var dir = (Directions)i;
                    if (dir.Opposite() == from)
                        continue;

                    var coords = current.Move(dir);
                    if (coords == end || Labyrinth[coords.Y][coords.X] == FREE)
                    {
                        Path.Add(coords);
                        from = dir;
                        current = coords;
                        break;
                    }
                }
            }
        }

        private static int GetBestShortcuts()
        {
            var result = 0;
            var maxWidth = Labyrinth[0].Length;
            var maxHeight = Labyrinth.Length;
            var end = Path.Last();

            for (int i = 0; i < Path.Count; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    var dir = (Directions)j;
                    var wall = Path[i].Move(dir);
                    var free = wall.Move(dir);
                    if (!free.IsOutOfBounds(maxWidth, maxHeight) && Labyrinth[wall.Y][wall.X] == WALL && (Labyrinth[free.Y][free.X] == FREE || free == end))
                    {
                        var skip = Path.IndexOf(free) - i - 2;
                        if (skip >= MINSKIP)
                            result++;
                    }
                }
            }

            return result;
        }

        private static int GetBestShortcutsForPart2()
        {
            var result = 0;

            for (int i = 0; i < Path.Count - MINSKIP - 1; i++)
            {
                for (int j = i+MINSKIP; j < Path.Count; j++)
                {
                    var a = Path[i];
                    var b = Path[j];
                    var cheatLength = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
                    var skip = j - i - cheatLength;
                    if (cheatLength <= 20 && skip >= MINSKIP)
                        result++;
                }
            }

            return result;
        }
    }
}
