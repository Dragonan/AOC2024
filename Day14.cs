using System;
using System.Drawing;
using System.Numerics;

namespace AOC2024
{
    public class EasterRobot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point Velocity { get; }

        public EasterRobot(int x, int y, Point velocity)
        {
            X = x;
            Y = y;
            Velocity = velocity;
        }
    }

    public class Day14
    {
        private static readonly Point PuzzleRoomSize = new (101,103);
        private static readonly Point TestRoomSize = new (11,7);
        private static readonly Point RoomSize = PuzzleRoomSize;

        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var robots = GetRobots();
            var room = Enumerable.Repeat(new int[] {}, RoomSize.Y).Select(r => Enumerable.Repeat(0, RoomSize.X).ToArray()).ToArray();

            foreach (var robot in robots)
            {
                var endX = (robot.X + (robot.Velocity.X*100)) % RoomSize.X;
                endX = endX < 0 ? RoomSize.X + endX : endX;
                var endY = (robot.Y + (robot.Velocity.Y*100)) % RoomSize.Y;
                endY = endY < 0 ? RoomSize.Y + endY : endY;
                room[endY][endX]++;
            }

            Graphics.Draw(room.Select(r => string.Join("",r).Replace('0','.')));

            var halfWidth = (RoomSize.X - 1) / 2;
            var halfHeight = (RoomSize.Y - 1) / 2;
            var a = room.Take(halfHeight).Select(r => r.Take(halfWidth)).SelectMany(r => r.Where(n => n != 0)).Sum();
            var b = room.Take(halfHeight).Select(r => r.Skip(halfWidth+1)).SelectMany(r => r.Where(n => n != 0)).Sum();
            var c = room.Skip(halfHeight+1).Select(r => r.Take(halfWidth)).SelectMany(r => r.Where(n => n != 0)).Sum();
            var d = room.Skip(halfHeight+1).Select(r => r.Skip(halfWidth+1)).SelectMany(r => r.Where(n => n != 0)).Sum();

            Console.WriteLine(a*b*c*d);
        }

        private static void SolvePart2()
        {
            var robots = GetRobots();
            var emptyRoom = Enumerable.Repeat(string.Join("",Enumerable.Repeat(' ', RoomSize.X)), RoomSize.Y);
            var steps = 0;

            var xPattern = 0; //The first step at which a vertical pattern is visible
            var yPattern = 0; //The first step at which a horizontal pattern is visible
            while(xPattern != yPattern)
            {
                if (xPattern < yPattern)
                    xPattern += RoomSize.X;
                else
                    yPattern += RoomSize.Y;
            }

            var SKIPFIRST = steps = xPattern - 1;
            foreach (var robot in robots)
            {
                robot.X = (robot.X + (SKIPFIRST*robot.Velocity.X)) % RoomSize.X;
                robot.X = robot.X < 0 ? RoomSize.X + robot.X : robot.X;
                robot.Y = (robot.Y + (SKIPFIRST*robot.Velocity.Y)) % RoomSize.Y;
                robot.Y = robot.Y < 0 ? RoomSize.Y + robot.Y : robot.Y;
            }


            Console.Clear();
            Console.WriteLine("Ready to start!");
            var key = Console.ReadKey().Key;
            while (key == ConsoleKey.Spacebar || key == ConsoleKey.Backspace)
            {
                var room = emptyRoom.Select(row => row).ToArray();
                var mod = key == ConsoleKey.Spacebar ? 1 : -1;
                steps += mod;
                foreach (var robot in robots)
                {
                    robot.X = (robot.X + (mod*robot.Velocity.X)) % RoomSize.X;
                    robot.X = robot.X < 0 ? RoomSize.X + robot.X : robot.X;
                    robot.Y = (robot.Y + (mod*robot.Velocity.Y)) % RoomSize.Y;
                    robot.Y = robot.Y < 0 ? RoomSize.Y + robot.Y : robot.Y;
                    room[robot.Y] = room[robot.Y].ReplaceAt(robot.X, '1');
                }
                Graphics.Draw(room, false);
                Console.WriteLine(steps);
                key = Console.ReadKey().Key;
            }
            
            Console.WriteLine(steps);
            Console.ReadLine();
        }

        private static List<EasterRobot> GetRobots()
        {
            var result = new List<EasterRobot>();
            var lines = PuzzleInput.Input;
            foreach (var line in lines)
            {

                var firstComa = line.IndexOf(',');
                var secondComa = line.LastIndexOf(',');
                var space = line.IndexOf(' ');
                result.Add(
                        new EasterRobot(int.Parse(line.Substring(2,firstComa-2)),
                        int.Parse(line.Substring(firstComa+1,space-firstComa-1)),
                    new (
                        int.Parse(line.Substring(space+3,secondComa-space-3)),
                        int.Parse(line.Substring(secondComa+1))
                )));
            }
            return result;
        }
    }
}
