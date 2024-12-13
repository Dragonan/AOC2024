using System;
using System.Drawing;
using System.Runtime.Intrinsics.X86;

namespace AOC2024
{
    public class ClawMachine
    {
        public Point A { get; }
        public Point B { get; }
        public long PrizeX { get; }
        public long PrizeY { get; }

        public ClawMachine(Point a, Point b, long prizeX, long prizeY)
        {
            A = a;
            B = b;
            PrizeX = prizeX;
            PrizeY = prizeY;
        }
    }

    public class Day13
    {
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var machines = GetMachines();
            var sum = 0;

            foreach (var machine in machines)
            {
                var maxA = Math.Min(Math.Min(machine.PrizeX / machine.A.X, machine.PrizeY / machine.A.Y), 100);
                var maxB = Math.Min(Math.Min(machine.PrizeX / machine.B.X, machine.PrizeY / machine.B.Y), 100);

                var winningCombo = 401;
                for (int pressA = 0; pressA <= maxA; pressA++)
                {
                    for (int pressB = 0; pressB <= maxB; pressB++)
                    {
                        var destX = machine.A.X * pressA + machine.B.X * pressB;
                        var destY = machine.A.Y * pressA + machine.B.Y * pressB;
                        if (destX == machine.PrizeX && destY == machine.PrizeY)
                            winningCombo = Math.Min(winningCombo, pressA*3 + pressB);
                        
                        if (destX > machine.PrizeX || destY > machine.PrizeY)
                            break;
                    }
                }
            
                if (winningCombo < 401)
                    sum += winningCombo;
            }

            Console.WriteLine(sum);
        }

        private static void SolvePart2()
        {
            var machines = GetMachines(true);
            var sum = 0D;

            foreach (var machine in machines)
            {
                var a = machine.A;
                var b = machine.B;
                var pX = machine.PrizeX;
                var pY = machine.PrizeY;

                var aSteps = (double)(b.Y*pX - b.X*pY)/(b.Y*a.X - b.X*a.Y);
                if (aSteps % 1 == 0)
                {
                    var bSteps = (double)(pX - a.X*aSteps)/b.X;
                    if (bSteps % 1 == 0)
                        sum += aSteps * 3 + bSteps;
                }
            }

            Console.WriteLine(sum);
        }

        private static List<ClawMachine> GetMachines(bool forPart2 = false)
        {
            var result = new List<ClawMachine>();
            var lines = PuzzleInput.Input;
            var adjustment = forPart2 ? 10000000000000 : 0;
            for (int i = 0; i < lines.Length; i+=4)
            {
                var aComa = lines[i].IndexOf(',');
                var bComa = lines[i+1].IndexOf(',');
                var prizeComa = lines[i+2].IndexOf(',');
                result.Add(new (
                    new (int.Parse(lines[i].Substring(12, aComa-12)), int.Parse(lines[i].Substring(aComa+4))),
                    new (int.Parse(lines[i+1].Substring(12, bComa-12)), int.Parse(lines[i+1].Substring(bComa+4))),
                    long.Parse(lines[i+2].Substring(9, prizeComa-9)) + adjustment, 
                    long.Parse(lines[i+2].Substring(prizeComa+4)) + adjustment
                ));
            }
            return result;
        }
    }
}
