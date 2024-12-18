using System;
using System.Drawing;

namespace AOC2024
{
    public class Day18
    {
        private const int MAPSIZE = 71;
        private const int FALLEN = 1024;
        private static Stack<Point> Blocks = new Stack<Point>();
        private static bool[,] CorruptedSpaces = new bool[MAPSIZE,MAPSIZE];
        private static List<DijkstraNode> Nodes = new List<DijkstraNode>();

        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            GenerateMap();
            var steps = GetShortestPath();
            Console.WriteLine(steps);
        }

        private static void SolvePart2()
        {
            GenerateMap();
            while(Blocks.Any())
            {
                var block = Blocks.Pop();
                Corrupt(block);
                if (!IsThereAPath())
                {
                    Console.WriteLine(block);
                    break;
                }
            }

        }

        private static void GenerateMap()
        {
            var lines = PuzzleInput.Input;
            for (int i = lines.Length-1; i >= 0; i--)
            {
                var numbers = lines[i].Split(',').Select(int.Parse);
                Blocks.Push(new (numbers.First(), numbers.Last()));
            }

            for (int i = 0; i < FALLEN; i++)
            {
                var block = Blocks.Pop();
                CorruptedSpaces[block.Y,block.X] = true;
            }

            for (int y = 0; y < MAPSIZE; y++)
            {
                for (int x = 0; x < MAPSIZE; x++)
                {
                    var coords = new Point(x, y);
                    if (!IsSafe(coords))
                        continue;
                    
                    var current = GetNode(coords);

                    var nextDirs = new [] { Directions.Right, Directions.Down };
                    foreach (var nextDir in nextDirs)
                    {
                        var nextLoc = coords.Move(nextDir);
                        if (IsSafe(nextLoc))
                        {
                            var neighbour = GetNode(nextLoc);
                            current.Paths.Add(new (neighbour, 1));
                            neighbour.Paths.Add(new (current, 1));
                        }
                    }
                }
            }
        }

        private static int GetShortestPath()
        {
            var startCoords = new Point(0,0);
            var endCoords = new Point(MAPSIZE-1,MAPSIZE-1);
            var startNode = GetNode(startCoords);
            startNode.Steps = 0;

            var toCheck = new List<DijkstraNode> { startNode };
            while(toCheck.Any())
            {
                var nextToCheck = new List<DijkstraNode>();
                foreach (var current in toCheck)
                {
                    foreach (var path in current.Paths)
                    {
                        var target = path.EndNode;
                        if (target.Visited)
                            continue;
                        
                        target.Steps = Math.Min(target.Steps, current.Steps + path.Length);
                        nextToCheck.Add(target);
                    }
                    current.Visited = true;
                }
                toCheck = nextToCheck.Distinct().OrderBy(n => n.Steps).ToList();
            }

            return GetNode(endCoords).Steps;
        }

        private static void Corrupt(Point coords)
        {
            CorruptedSpaces[coords.Y,coords.X] = true;
            var node = GetNode(coords);
            foreach (var neighbour in node.Paths)
                neighbour.EndNode.Paths.RemoveAll(p => p.EndNode == node);
            node.Paths.Clear();
            Nodes.Remove(node);
        }
        private static bool IsThereAPath()
        {
            Nodes.ForEach(n => n.Visited = false);
            var startCoords = new Point(0,0);
            var endCoords = new Point(MAPSIZE-1,MAPSIZE-1);
            var startNode = GetNode(startCoords);

            var toCheck = new List<DijkstraNode> { startNode };
            while(toCheck.Any())
            {
                var nextToCheck = new List<DijkstraNode>();
                foreach (var current in toCheck)
                {
                    foreach (var path in current.Paths)
                    {
                        var target = path.EndNode;
                        if (target.Visited)
                            continue;
                        
                        if (target.Coords == endCoords)
                            return true;
                        
                        nextToCheck.Add(target);
                    }
                    current.Visited = true;
                }
                toCheck = nextToCheck.Distinct().ToList();
            }

            return false;
        }

        private static bool IsSafe(Point coords) =>  !coords.IsOutOfBounds(MAPSIZE,MAPSIZE) && !CorruptedSpaces[coords.Y,coords.X];

        private static DijkstraNode GetNode(Point coords)
        {
            var node = Nodes.FirstOrDefault(n => n.Coords == coords);
            if (node == null)
            {
                node = new DijkstraNode(coords);
                Nodes.Add(node);
            }
            return node;
        }
    }
}
