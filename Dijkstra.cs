using System;
using System.Drawing;

namespace AOC2024
{
    public class DijkstraNode
    {
        public Point Coords { get; }
        public Directions? Dir { get; }
        public List<DijkstraPath> Paths { get; }
        public int Steps { get; set; }
        public bool Visited { get; set; }
        
        public DijkstraNode(Point coords, Directions? dir = null)
        {
            Coords = coords;
            Dir = dir;
            Paths = new List<DijkstraPath>();
            Steps = int.MaxValue;
        }

        public override string ToString()
        {
            return Coords.ToString() + (Dir != null ? (" " + Dir.ToString()) : "");
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
}
