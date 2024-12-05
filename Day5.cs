using System;

namespace AOC2024
{
    public class PageNode
    {
        public int Number { get; set; }

        public List<int> NextPages { get; set; }

        public PageNode(int number)
        {
            Number = number;
            NextPages = new List<int>();
        }
    }

    public static class Day5
    {
        public static void Solve()
        {
            var input = PuzzleInput.Input;
            var emptyLine = Array.IndexOf(input, "");
            var orderingRules = input.Take(emptyLine).ToArray();
            int[][] updates = input.Skip(emptyLine+1).Select(o => o.Split(',').Select(int.Parse).ToArray()).ToArray();
            var pageNodes = GetPageRules(orderingRules);

            SolvePart2(updates, pageNodes);
        }

        private static void SolvePart1(int[][] updates, List<PageNode> pageNodes)
        {
            var sum = 0;

            foreach (var update in updates)
            {
                var isValid = true;
                for (int i = 0; i < update.Length ; i++)
                {
                    var page = update[i];
                    var nextPages = update.Skip(i + 1);
                    if (pageNodes.Any(n => nextPages.Contains(n.Number) && n.NextPages.Contains(page)))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                    sum += update[update.Length/2];
            }

            Console.WriteLine(sum);
        }

        private static void SolvePart2(int[][] updates, List<PageNode> pageNodes)
        {
            var sum = 0;

            foreach (var update in updates)
            {
                var isReordered = false;
                var orderedItems = update.ToList();
                for (int i = 0; i < orderedItems.Count ; i++)
                {
                    var page = orderedItems[i];
                    var nextPages = orderedItems.Skip(i + 1);
                    var outOfPlace = pageNodes.Where(n => nextPages.Contains(n.Number) && n.NextPages.Contains(page)).Select(n => n.Number);
                    if (outOfPlace.Any())
                    {
                        foreach (var toMove in outOfPlace)
                        {
                            orderedItems.Remove(toMove);
                            orderedItems.Insert(i, toMove);
                        }

                        isReordered = true;
                        i--;
                    }
                }

                if (isReordered)
                    sum += orderedItems[orderedItems.Count/2];
            }

            Console.WriteLine(sum);
        }

        private static List<PageNode> GetPageRules(string[] orderingRules)
        {
            var result = new List<PageNode>();

            foreach (var rule in orderingRules)
            {
                var numbers = rule.Split('|').Select(int.Parse).ToArray();

                if (!result.Any(n => n.Number == numbers[0]))
                    result.Add(new PageNode(numbers[0]));
                if (!result.Any(n => n.Number == numbers[1]))
                    result.Add(new PageNode(numbers[1]));

                result.First(n => n.Number == numbers[0]).NextPages.Add(numbers[1]);
            }

            return result;
        }
    }
}
