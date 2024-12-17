using System;
using System.Drawing;

namespace AOC2024
{
    public struct PointWithDir : IEquatable<PointWithDir>
    {
        public Point Point { get; set; }
        public Directions Dir { get; set; }

        public PointWithDir(Point point, Directions dir)
        {
            Point = point;
            Dir = dir;
        }

        public bool Equals(PointWithDir other)
        {
            return this.Point == other.Point && this.Dir == other.Dir;
        }

        public static bool operator ==(PointWithDir a, PointWithDir b) =>  a.Equals(b);

        public static bool operator !=(PointWithDir a, PointWithDir b) => !a.Equals(b);
    }
}
