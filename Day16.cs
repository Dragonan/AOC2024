using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AOC2024
{
    public class DijkstraNode
    {
        public PointWithDir Place { get; }
        public List<DijkstraPath> Paths { get; }
        public int Steps { get; set; }
        public bool Visited { get; set; }
        
        public DijkstraNode(PointWithDir place)
        {
            Place = place;
            Paths = new List<DijkstraPath>();
            Steps = int.MaxValue;
        }

        public override string ToString()
        {
            return Place.Point.ToString() + " " + Place.Dir.ToString();
        }
    }

    public class DijkstraPath
    {
        public DijkstraNode EndNode { get; }
        public int Length { get; }

        public DijkstraPath(DijkstraNode end, int length)
        {
            EndNode = end;
            Length = length;
        }
    }

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
            EndCoords = new Point(Labyrinth[0].Length-2, 1);

            SolvePart2();
        }

        private static void SolvePart1()
        {
            MapLabyrinth();
            GetPathToExit();
            var endNodeSteps = Nodes.Where(n => n.Place.Point == EndCoords).Select(n => n.Steps).Order().First();
            Console.WriteLine(endNodeSteps);
        }

        private static void SolvePart2()
        {
            MapLabyrinth();
            GetPathToExit();
            var realEndNodes = Nodes.Where(n => n.Place.Point == EndCoords).GroupBy(n => n.Steps).OrderBy(g => g.Key).First();
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

            //Graphics.DrawWithColor(Labyrinth, winningNodes.Select(n => n.Place.Point).Distinct().ToList(), ConsoleColor.DarkGray, ConsoleColor.DarkYellow);
            Console.WriteLine(winningNodes.DistinctBy(n => n.Place.Point).Count());
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
                    var existing = Nodes.Where(n => n.Place.Point == coords).ToList();

                    var nextDirs = new [] { Directions.Right, Directions.Down };
                    foreach (var nextDir in nextDirs)
                    {
                        var nextLoc = coords.Move(nextDir);
                        if (Labyrinth[nextLoc.Y][nextLoc.X] != WALL)
                        {
                            var opposite = nextDir.Opposite();
                            var nodeA = new DijkstraNode(new (coords, nextDir));
                            var nodeB = new DijkstraNode(new (nextLoc, opposite));
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
                            var length = nodeA.Place.Dir == nodeB.Place.Dir.Opposite() ? 0 : 1000;
                            nodeA.Paths.Add(new (nodeB, length));
                            nodeB.Paths.Add(new (nodeA, length));
                        }
                }
            }

            
            var startingDirs = Nodes.Where(n => n.Place.Point == StartCoords);
            if (!startingDirs.Any(n => n.Place.Dir == Directions.Right))
            {
                var startingNode = new DijkstraNode(new (StartCoords, Directions.Right));
                Nodes.Add(startingNode);
                var otherNode = startingDirs.First();
                startingNode.Paths.Add(new (otherNode, 1000));
                otherNode.Paths.Add(new (startingNode, 1000));
            }
        }

        private static bool IsCorridor(DijkstraNode node)
        {
            return node.Paths.Count == 2 && node.Place.Point != StartCoords || node.Place.Point != EndCoords;
        }

        private static void GetPathToExit()
        {
            var startPlace = new PointWithDir(StartCoords, Directions.Right);
            var startNode = Nodes.First(n => n.Place == startPlace);
            startNode.Steps = 0;
            
            var toCheck = new List<DijkstraNode> { startNode };
            while (toCheck.Any())
            {
                var current = toCheck.First();
                if (current.Place.Point == EndCoords)
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
