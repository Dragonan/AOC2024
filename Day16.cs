using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AOC2024
{
    public class Day16
    {
        private const char WALL = '#';
        private static string[] Labyrinth;
        private static List<DijkstraNode> Nodes;
        private static Point StartCoords;
        private static Point EndCoords;
        public static void Solve()
        {
            Labyrinth = PuzzleInput.Input;
            Nodes = new List<DijkstraNode>();
            StartCoords = new (1, Labyrinth.Length-2);
            EndCoords = new (Labyrinth[0].Length-2, 1);

            SolvePart2();
        }

        private static void SolvePart1()
        {
            MapLabyrinth();
            GetPathToExit();
            var endNodeSteps = Nodes.Where(n => n.Coords == EndCoords).Select(n => n.Steps).Order().First();
            Console.WriteLine(endNodeSteps);
        }

        private static void SolvePart2()
        {
            MapLabyrinth();
            GetPathToExit();
            var realEndNodes = Nodes.Where(n => n.Coords == EndCoords).GroupBy(n => n.Steps).OrderBy(g => g.Key).First();
            var toCheck = new List<DijkstraNode>(realEndNodes);
            var winningNodes = new List<DijkstraNode>();

            while (toCheck.Any())
            {
                var node = toCheck.First();
                
                foreach (var path in node.Paths)
                    if (node.Steps == path.EndNode.Steps + path.Length)
                        toCheck.Add(path.EndNode);

                winningNodes.Add(node);
                toCheck = toCheck.Skip(1).Distinct().Where(n => !winningNodes.Contains(n)).ToList();
            }

            //Graphics.DrawWithColor(Labyrinth, winningNodes.Select(n => n.Coords).Distinct().ToList(), ConsoleColor.DarkGray, ConsoleColor.DarkYellow);
            Console.WriteLine(winningNodes.DistinctBy(n => n.Coords).Count());
        }

        private static void MapLabyrinth()
        {
            for (int y = 1; y < Labyrinth.Length - 1; y++)
            {
                for (int x = 1; x < Labyrinth[y].Length - 1; x++)
                {
                    if (Labyrinth[y][x] == WALL)
                        continue;
                    
                    var coords = new Point(x, y);
                    var existing = Nodes.Where(n => n.Coords == coords).ToList();

                    var nextDirs = new [] { Directions.Right, Directions.Down };
                    foreach (var nextDir in nextDirs)
                    {
                        var nextLoc = coords.Move(nextDir);
                        if (Labyrinth[nextLoc.Y][nextLoc.X] != WALL)
                        {
                            var opposite = nextDir.Opposite();
                            var nodeA = new DijkstraNode(coords, nextDir);
                            var nodeB = new DijkstraNode(nextLoc, opposite);
                            nodeA.Paths.Add(new (nodeB, 1));
                            nodeB.Paths.Add(new (nodeA, 1));
                            Nodes.Add(nodeA);
                            Nodes.Add(nodeB);
                            existing.Add(nodeA);
                        }
                    }
                    for (int i = 0; i < existing.Count - 1; i++)
                        for (int j = i+1; j < existing.Count; j++)
                        {
                            var nodeA = existing[i];
                            var nodeB = existing[j];
                            var length = nodeA.Dir == nodeB.Dir.Value.Opposite() ? 0 : 1000;
                            nodeA.Paths.Add(new (nodeB, length));
                            nodeB.Paths.Add(new (nodeA, length));
                        }
                }
            }

            
            var startingDirs = Nodes.Where(n => n.Coords == StartCoords);
            if (!startingDirs.Any(n => n.Dir == Directions.Right))
            {
                var startingNode = new DijkstraNode(StartCoords, Directions.Right);
                Nodes.Add(startingNode);
                var otherNode = startingDirs.First();
                startingNode.Paths.Add(new (otherNode, 1000));
                otherNode.Paths.Add(new (startingNode, 1000));
            }
        }

        private static void GetPathToExit()
        {
            var startNode = Nodes.First(n => n.Coords == StartCoords && n.Dir == Directions.Right);
            startNode.Steps = 0;
            
            var toCheck = new List<DijkstraNode> { startNode };
            while (toCheck.Any())
            {
                var current = toCheck.First();
                if (current.Coords == EndCoords)
                {
                    toCheck.RemoveAt(0);
                    continue;
                }
                foreach (var path in current.Paths)
                {
                    var node = path.EndNode;
                    if (node.Visited)
                        continue;
                    
                    toCheck.Add(node);
                    node.Steps = Math.Min(current.Steps + path.Length, node.Steps);
                }
                current.Visited = true;
                toCheck = toCheck.Skip(1).Distinct().OrderBy(n => n.Steps).ToList();
            }
        }
    }
}
