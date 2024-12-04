using System;
using System.Drawing;

namespace AOC2024
{
    public class Day4
    {
        private const string XMAS = "XMAS";

        public static void Solve()
        {
            SolvePart2();
        }
        
        private static void SolvePart1()
        {
            var wordsearch = PuzzleInput.Input;
            var startPoints = FindStartPoints(wordsearch, 'X');
            var maxX = wordsearch[0].Length;
            var maxY = wordsearch.Length;
            
            var count = 0;

            foreach (var start in startPoints)
            {
                var endX = start.X + 3;
                var endY = start.Y + 3;
                var startX = start.X - 3;
                var startY = start.Y - 3;

                // left to right
                if (endX < maxX && CheckWord(wordsearch, start, 1, 0))
                    count++;
                // right to left
                if (startX > -1 && CheckWord(wordsearch, start, -1, 0))
                    count++;
                // downwards
                if (endY < maxY && CheckWord(wordsearch, start, 0, 1))
                    count++;
                // upwards
                if (startY > -1 && CheckWord(wordsearch, start, 0, -1))
                    count++;
                //diagonal UL
                if (startX > -1 && startY > -1 && CheckWord(wordsearch, start, -1, -1))
                    count++;
                //diagonal UR
                if (endX < maxX && startY > -1 && CheckWord(wordsearch, start, 1, -1))
                    count++;
                //diagonal DL
                if (startX > -1 && endY < maxY && CheckWord(wordsearch, start, -1, 1))
                    count++;
                //diagonal DR
                if (endX < maxX && endY < maxY && CheckWord(wordsearch, start, 1, 1))
                    count++;
            }

            Console.WriteLine(count);
        }
        
        private static void SolvePart2()
        {
            
            var wordsearch = PuzzleInput.Input;
            var startPoints = FindStartPoints(wordsearch, 'A');
            var maxX = wordsearch[0].Length;
            var maxY = wordsearch.Length;
            
            var count = 0;

            foreach (var start in startPoints)
            {
                if (start.X == 0 || start.X == maxX - 1 || start.Y == 0 || start.Y == maxY - 1)
                    continue;

                var topLeft = wordsearch[start.Y - 1][start.X - 1];
                var topRight = wordsearch[start.Y - 1][start.X + 1];
                var bottomLeft = wordsearch[start.Y + 1][start.X - 1];
                var bottomRight = wordsearch[start.Y + 1][start.X + 1];

                if (((topLeft == 'M' && bottomRight == 'S') || (topLeft == 'S' && bottomRight == 'M')) &&
                    ((topRight == 'M' && bottomLeft == 'S') || (topRight == 'S' && bottomLeft == 'M')))
                    count++;
            }

            Console.WriteLine(count);
        }

        private static List<Point> FindStartPoints(string[] wordsearch, char letter)
        {
            var result = new List<Point>();

            for (int i = 0; i < wordsearch.Length; i++)
            {
                for (int j = 0; j < wordsearch[i].Length; j++)
                {
                    if (wordsearch[i][j] == letter)
                        result.Add(new (j, i));
                }
            }

            return result;
        }

        private static bool CheckWord(string[] wordsearch, Point start, int xMod, int yMod)
        {
            for (int i = 1; i < XMAS.Length; i++)
            {
                if (wordsearch[start.Y + (yMod*i)][start.X + (xMod*i)] != XMAS[i])
                    return false;
            }

            return true;
        }
    }
}
