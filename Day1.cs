using System;

namespace AOC2024
{
    public static class Day1
    {
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var lists = GetLists();
            var summedDistances = 0;

            for (int i = 0; i < lists.Left.Count; i++)
            {
                summedDistances += Math.Abs(lists.Left[i] - lists.Right[i]);
            }

            Console.WriteLine(summedDistances);
        }

        private static void SolvePart2()
        {
            var lists = GetLists();
            var score = 0;

            for (int i = 0; i < lists.Left.Count; i++)
            {
                score += lists.Left[i] * lists.Right.Count(r => r == lists.Left[i]);
            }

            Console.WriteLine(score);
        }

        private static (List<int> Left, List<int> Right) GetLists()
        {
            var lists = (Left: new List<int>(), Right: new List<int>());
            
            var lines = PuzzleInput.Input;
            
            foreach (var line in lines)
            {
                var values = line.Split("   ");
                lists.Left.Add(int.Parse(values[0]));
                lists.Right.Add(int.Parse(values[1]));
            }

            lists.Left.Sort();
            lists.Right.Sort();

            return lists;
        } 
    }
}
