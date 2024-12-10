using System;
using System.Drawing;

namespace AOC2024
{
    public class HikingNode
    {
        public int ID { get; }
        public int Number { get; }
        public Point Coords { get; }

        public List<HikingNode> NextSteps { get; }

        public HikingNode(int id, int number, Point coords)
        {
            ID = id;
            Number = number;
            Coords = coords;
            NextSteps = new List<HikingNode>();
        }
    }

    public class Day10
    {
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var map = PuzzleInput.Input;
            var trailHeads = GetNodes(map);
            var sum = GetTotalScore(trailHeads);
            Console.WriteLine(sum);
        }

        private static void SolvePart2()
        {
            var map = PuzzleInput.Input;
            var trailHeads = GetNodes(map);
            var sum = GetTotalScore(trailHeads, true);
            Console.WriteLine(sum);
        }

        private static List<HikingNode> GetNodes(string[] map)
        {
            var result = new List<HikingNode>();
            var nodeID = 1;
            var lastRow = new List<HikingNode>();
            var currentRow = new List<HikingNode>();
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    var node = new HikingNode(nodeID++, int.Parse(map[i][j].ToString()), new (j, i));
                    
                    if (i > 0)
                    {
                        var upperNode = lastRow.First(n => n.Coords.X == j);
                        if (upperNode.Number == node.Number + 1)
                            node.NextSteps.Add(upperNode);
                        else if (upperNode.Number == node.Number - 1)
                            upperNode.NextSteps.Add(node);
                    }

                    if (j > 0)
                    {
                        var leftNode = currentRow.First(n => n.Coords.X == j - 1);
                        if (leftNode.Number == node.Number + 1)
                            node.NextSteps.Add(leftNode);
                        else if (leftNode.Number == node.Number - 1)
                            leftNode.NextSteps.Add(node);
                    }

                    currentRow.Add(node);
                    if (node.Number == 0)
                        result.Add(node);
                }
                    
                lastRow = new (currentRow);
                currentRow.Clear();
            }

            return result;
        }

        private static int GetTotalScore(List<HikingNode> trailHeads, bool countDistinct = false)
        {
            var result = 0;

            foreach (var head in trailHeads)
            {
                var checkedNodeIds = new List<int>();
                var nodesToCheck = new List<HikingNode> { head };
                var score = 0;

                while (nodesToCheck.Any())
                {
                    var nextNodes = new List<HikingNode>();
                    foreach (var node in nodesToCheck)
                    {
                        checkedNodeIds.Add(node.ID);

                        if (node.Number == 9)
                        {
                            score++;
                            continue;
                        }

                        foreach (var next in node.NextSteps)
                        {
                            if (!countDistinct && (checkedNodeIds.Contains(next.ID) || nextNodes.Contains(next)))
                                continue;

                            nextNodes.Add(next);
                        }
                    }
                    nodesToCheck = nextNodes;
                }

                result += score;
            }

            return result;
        }
    }
}
