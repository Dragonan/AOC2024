using System;
using System.Collections.Immutable;
using System.Drawing;

namespace AOC2024
{
    public class Day15
    {
        private const char WALL = '#';
        private const char CRATE = 'O';
        private const char FREE = '.';
        private const char ROBOT = '@';
        private const char LEFT_CRATE = '[';
        private const char RIGHT_CRATE = ']';

        private static List<Directions> RobotMoves;
        private static Point RobotPosition;
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var map = GetMapAndSetRobotMoves();
            var robot = RobotPosition;

            foreach (var move in RobotMoves)
            {
                var nextSpace = robot.Move(move);
                if (nextSpace.IsOutOfBounds(map[0].Length, map.Length))
                    continue;

                var nextChar = map[nextSpace.Y][nextSpace.X];
                if (nextChar == WALL)
                    continue;

                if (nextChar == FREE)
                {
                    map[nextSpace.Y][nextSpace.X] = ROBOT;
                    map[robot.Y][robot.X] = FREE;
                    robot = nextSpace;
                    continue;
                }
                
                if (nextChar == CRATE)
                {
                    var nextNextSpace = nextSpace.Move(move);
                    while (!nextNextSpace.IsOutOfBounds(map[0].Length, map.Length))
                    {
                        var nextNextChar = map[nextNextSpace.Y][nextNextSpace.X];
                        if (nextNextChar == WALL)
                            break;
                        if (nextNextChar == CRATE)
                        {
                            nextNextSpace = nextNextSpace.Move(move);
                            continue;
                        }
                        if (nextNextChar == FREE)
                        {
                            map[nextNextSpace.Y][nextNextSpace.X] = CRATE;
                            map[nextSpace.Y][nextSpace.X] = ROBOT;
                            map[robot.Y][robot.X] = FREE;
                            robot = nextSpace;
                            break;
                        }
                    }
                }
            }

            var sum = 0;
            for (int y = 0; y < map.Length; y++)
                for (int x = 0; x < map[y].Length; x++)
                    if (map[y][x] == CRATE)
                        sum += 100*(y+1) + x+1;
            Console.WriteLine(sum);
        }

        private static void SolvePart2()
        {
            
            var map = GetMapAndSetRobotMoves(true);
            var robot = RobotPosition;

            foreach (var move in RobotMoves)
            {
                var nextSpace = robot.Move(move);
                if (nextSpace.IsOutOfBounds(map[0].Length, map.Length))
                    continue;

                var nextChar = map[nextSpace.Y][nextSpace.X];
                if (nextChar == WALL)
                    continue;

                if (nextChar == FREE)
                {
                    map[nextSpace.Y][nextSpace.X] = ROBOT;
                    map[robot.Y][robot.X] = FREE;
                    robot = nextSpace;
                    continue;
                }
                
                if (nextChar == LEFT_CRATE || nextChar == RIGHT_CRATE)
                {
                    if (move == Directions.Left || move == Directions.Right)
                    {
                        var nextNextSpace = nextSpace.Move(move).Move(move);
                        while (!nextNextSpace.IsOutOfBounds(map[0].Length, map.Length))
                        {
                            var nextNextChar = map[nextNextSpace.Y][nextNextSpace.X];
                            if (nextNextChar == WALL)
                                break;
                            if (nextNextChar == LEFT_CRATE || nextNextChar == RIGHT_CRATE)
                            {
                                nextNextSpace = nextNextSpace.Move(move).Move(move);
                                continue;
                            }
                            if (nextNextChar == FREE)
                            {
                                if (move == Directions.Left)
                                    for (int x = nextNextSpace.X; x < nextSpace.X; x+=2)
                                    {
                                        map[nextSpace.Y][x] = LEFT_CRATE;
                                        map[nextSpace.Y][x+1] = RIGHT_CRATE;
                                    }
                                else
                                    for (int x = nextNextSpace.X; x > nextSpace.X; x-=2)
                                    {
                                        map[nextSpace.Y][x-1] = LEFT_CRATE;
                                        map[nextSpace.Y][x] = RIGHT_CRATE;
                                    }
                                map[nextSpace.Y][nextSpace.X] = ROBOT;
                                map[robot.Y][robot.X] = FREE;
                                robot = nextSpace;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var nextSpace2 = nextSpace.Move(nextChar == LEFT_CRATE ? Directions.Right : Directions.Left);
                        var toCheck = new List<Point> { nextSpace, nextSpace2 };
                        var toMove = new List<Point>();
                        var isBlocked = false;
                        while (!isBlocked && toCheck.Any())
                        {
                            var nextToCheck = new List<Point>();
                            foreach (var spaceToCheck in toCheck)
                            {
                                var nextNextSpace = spaceToCheck.Move(move);
                                char nextNextChar;
                                if (nextNextSpace.IsOutOfBounds(map[0].Length, map.Length) ||
                                    (nextNextChar = map[nextNextSpace.Y][nextNextSpace.X]) == WALL)
                                {
                                    isBlocked = true;
                                    break;
                                }

                                if (nextNextChar == LEFT_CRATE || nextNextChar == RIGHT_CRATE)
                                {
                                    nextToCheck.Add(nextNextSpace);
                                    nextToCheck.Add(nextNextSpace.Move(nextNextChar == LEFT_CRATE ? Directions.Right : Directions.Left));
                                    continue;
                                }
                            }
                            toMove.AddRange(toCheck);
                            toCheck = nextToCheck.Distinct().ToList();
                        }
                        if (isBlocked)
                            continue;
                        
                        toMove = toMove.Distinct().Reverse().ToList();
                        var yMod = move == Directions.Up ? -1 : 1;
                        foreach (var crateToMove in toMove)
                        {
                            map[crateToMove.Y + yMod][crateToMove.X] = map[crateToMove.Y][crateToMove.X];
                            map[crateToMove.Y][crateToMove.X] = FREE;
                        }
                        map[nextSpace.Y][nextSpace.X] = ROBOT;
                        map[robot.Y][robot.X] = FREE;
                        robot = nextSpace;
                    }
                }
            }

            var sum = 0;
            for (int y = 0; y < map.Length; y++)
                for (int x = 0; x < map[y].Length; x++)
                    if (map[y][x] == LEFT_CRATE)
                        sum += 100*(y+1) + x+2;
            Console.WriteLine(sum);
        }

        private static char[][] GetMapAndSetRobotMoves(bool isForPart2 = false)
        {
            var input = PuzzleInput.Input.ToList();

            var separator = input.IndexOf("");
            var mult = isForPart2 ? 2 : 1;
            var map = new char[separator-2][];

            for (int i = 1; i < separator-1; i++)
            {
                map[i-1] = new char[mult*(input[i].Length-2)];
                for (int j = 1; j < input[i].Length-1; j++)
                {
                    var newIndex = i-1;
                    var newJndex = mult*(j-1);
                    var leftValue = input[i][j];
                    map[newIndex][newJndex] = leftValue;
                    if (leftValue == ROBOT)
                        RobotPosition = new Point(newJndex,newIndex);
                    if (isForPart2)
                    {
                        map[newIndex][newJndex+1] = 
                            leftValue == WALL ? WALL :
                            leftValue == CRATE ? RIGHT_CRATE :
                            FREE;
                        
                        if (leftValue == CRATE)
                            map[newIndex][newJndex] = LEFT_CRATE;
                    }
                }
            }

            RobotMoves = new List<Directions>();
            for (int i = separator+1; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    var arrow = input[i][j];
                    RobotMoves.Add(
                        arrow == '^' ? Directions.Up :
                        arrow == '>' ? Directions.Right :
                        arrow == 'v' ? Directions.Down :
                        Directions.Left
                    );
                }
            }

            return map;
        }
    }
}
