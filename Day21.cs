using System;

namespace AOC2024
{
    public class Day21
    {
        private static string [] Codes;
        private static Dictionary<string,long> KeypadRobot;
        public static void Solve()
        {
            Codes = PuzzleInput.Input;
            SolvePart2();
        }

        private static void SolvePart1()
        {
            SetBasicRobotMoves();

            var sum = 0L;
            foreach (var code in Codes)
            {
                var steps = KeypadRobot["A" + code[0]];
                for (int i = 1; i < code.Length; i++)
                    steps += KeypadRobot[code[i-1].ToString() + code[i]];

                Console.WriteLine(code + ": " + steps);
                sum += steps * int.Parse(code.Substring(0,3));
            }

            Console.WriteLine(sum);
        }

        private static void SolvePart2()
        {
            SetBasicRobotMoves(25);

            var sum = 0L;
            foreach (var code in Codes)
            {
                var steps = KeypadRobot["A" + code[0]];
                for (int i = 1; i < code.Length; i++)
                    steps += KeypadRobot[code[i-1].ToString() + code[i]];

                Console.WriteLine(code + ": " + steps);
                sum += steps * int.Parse(code.Substring(0,3));
            }

            Console.WriteLine(sum);
        }

        private static void SetBasicRobotMoves(int midRobots = 2)
        {
            var movePaths = new Dictionary<string, string[]>
            {
                { "^", new [] { "^A" } },
                { "^^", new [] { "^^A" } },
                { "^^^", new [] { "^^^A" } },
                { "v", new [] { "vA" } },
                { "vv", new [] { "vvA" } },
                { "vvv", new [] { "vvvA" } },
                { ">", new [] { ">A" } },
                { "<<", new [] { "<<A" } },
                { "<", new [] { "<A" } },
                { ">>", new [] { ">>A" } },
                { "^<", new [] { "^<A", "<^A" } },
                { "^>", new [] { "^>A", ">^A" } },
                { "v<", new [] { "v<A", "<vA" } },
                { "v>", new [] { "v>A", ">vA" } },
                { "^<<", new [] { "^<<A", "<<^A", "<^<A" } },
                { "^>>", new [] { "^>>A", ">>^A", ">^>A" } },
                { "v<<", new [] { "v<<A", "<<vA", "<v<A" } },
                { "v>>", new [] { "v>>A", ">>vA", ">v>A" } },
                { "^^<", new [] { "^^<A", "<^^A", "^<^A" } },
                { "^^>", new [] { "^^>A", ">^^A", "^>^A" } },
                { "vv<", new [] { "vv<A", "<vvA", "v<vA" } },
                { "vv>", new [] { "vv>A", ">vvA", "v>vA" } },
                { "^^<<", new [] { "^^<<A", "<<^^A", "<^^<A", "^<<^A" } },
                { "^^>>", new [] { "^^>>A", ">>^^A", ">^^>A", "^>>^A" } },
                { "vv<<", new [] { "vv<<A", "<<vvA", "<vv<A", "v<<vA" } },
                { "vv>>", new [] { "vv>>A", ">>vvA", ">vv>A", "v>>vA" } },
                { "^^^<", new [] { "^^^<A", "<^^^A", "^<^^A" } },
                { "^^^>", new [] { "^^^>A", ">^^^A", "^>^^A" } },
                { "vvv<", new [] { "vvv<A", "<vvvA", "v<vvA" } },
                { "vvv>", new [] { "vvv>A", ">vvvA", "v>vvA" } },

                { "L^<", new [] { "^<A" } },
                { "L^>", new [] { ">^A" } },
                { "Lv<", new [] { "v<A" } },
                { "Lv>", new [] { ">vA" } },
                { "L^<<", new [] { "<^<A", "^<<A" } },
                { "L^>>", new [] { ">>^A", ">^>A"} },
                { "Lv<<", new [] { "v<<A", "<v<A"} },
                { "Lv>>", new [] { ">>vA", ">v>A" } },
                { "L^^<", new [] { "^^<A", "^<^A" } },
                { "Lvv>", new [] { ">vvA", "v>vA" } },
                { "L^^<<", new [] { "^^<<A", "^<<^A", "<^<^A", "<^^<A" } },
                { "Lvv>>", new [] { ">>vvA", "v>>vA", "v>v>A", ">vv>A" } },
                { "L^^^<", new [] { "^^^<A", "^<^^A", } },
                { "Lvvv>", new [] { ">vvvA", "v>vvA", } },
                { "L^^^<<", new [] { "^^^<<A", "<^^^<A", "<^<^^A", "^<<^^A" } },
                { "Lvvv>>", new [] { ">>vvvA", ">vvv>A", "v>vv>A", "v>>vvA" } }
            };

            var directRobot = new Dictionary<string, long>
            {
                { "A^", 2 }, { "Av", 3 }, { "A<", 4 }, { "A>", 2 },
                { "^A", 2 }, { "^<", 3 }, { "^>", 3 },
                { "vA", 3 }, { "v<", 2 }, { "v>", 2 },
                { "<A", 4 }, { "<^", 3 }, { "<v", 2 },
                { ">A", 2 }, { ">^", 3 }, { ">v", 2 }
            };

            var midRobot = directRobot;
            var midSteps = new Dictionary<string, long>();
            for (int r = 0; r < midRobots; r++)
            {
                foreach (var movement in movePaths)
                {
                    var paths = new List<long>();
                    foreach (var steps in movement.Value)
                    {
                        var path = midRobot["A" + steps[0]];
                        for (int i = 1; i < steps.Length; i++)
                            path += steps[i-1] == steps[i] ? 1 : midRobot[steps[i-1].ToString() + steps[i]];
                        paths.Add(path);
                    }
                    midSteps[movement.Key] = paths.Min();
                }
                midRobot = new Dictionary<string, long>
                {
                    { "A^", midSteps["<"] }, { "Av", midSteps["v<"] }, { "A<", midSteps["Lv<<"] }, { "A>", midSteps["v"] },
                    { "^A", midSteps[">"] }, { "^<", midSteps["Lv<"] }, { "^>", midSteps["v>"] },
                    { "vA", midSteps["^>"] }, { "v<", midSteps["<"] }, { "v>", midSteps[">"] },
                    { "<A", midSteps["L^>>"] }, { "<^", midSteps["L^>"] }, { "<v", midSteps[">"] },
                    { ">A", midSteps["^"] }, { ">^", midSteps["^<"] }, { ">v", midSteps["<"] }
                };
            }

            KeypadRobot = new Dictionary<string, long>
            {
                { "A7", midSteps["L^^^<<"] },   { "A8", midSteps["^^^<"] }, { "A9", midSteps["^^^"] },
                { "A4", midSteps["L^^<<"] },    { "A5", midSteps["^^<"] },  { "A6", midSteps["^^"] },
                { "A1", midSteps["L^<<"] },     { "A2", midSteps["^<"] },   { "A3", midSteps["^"] },
                                                { "A0", midSteps["<"] },

                { "07", midSteps["L^^^<"] },    { "08", midSteps["^^^"] },  { "09", midSteps["^^^>"] },
                { "04", midSteps["L^^<"] },     { "05", midSteps["^^"] },   { "06", midSteps["^^>"] }, 
                { "01", midSteps["L^<"] },      { "02", midSteps["^"] },    { "03", midSteps["^>"] },  
                                                                            { "0A", midSteps[">"] },

                { "17", midSteps["^^"] },       { "18", midSteps["^^>"] },  { "19", midSteps["^^>>"] }, 
                { "14", midSteps["^"] },        { "15", midSteps["^>"] },   { "16", midSteps["^>>"] },  
                                                { "12", midSteps[">"] },    { "13", midSteps[">>"] },
                                                { "10", midSteps["Lv>"] },  { "1A", midSteps["Lv>>"] },

                { "27", midSteps["^^<"] },      { "28", midSteps["^^"] },   { "29", midSteps["^^>"] }, 
                { "24", midSteps["^<"] },       { "25", midSteps["^"] },    { "26", midSteps["^>"] },  
                { "21", midSteps["<"] },                                    { "23", midSteps[">"] },
                                                { "20", midSteps["v"] },    { "2A", midSteps["v>"] },

                { "37", midSteps["^^<<"] },     { "38", midSteps["^^<"] },  { "39", midSteps["^^"] }, 
                { "34", midSteps["^<<"] },      { "35", midSteps["^<"] },   { "36", midSteps["^"] },  
                { "31", midSteps["<<"] },       { "32", midSteps["<"] },
                                                { "30", midSteps["v<"] },   { "3A", midSteps["v"] },

                { "47", midSteps["^"] },        { "48", midSteps["^>"] },   { "49", midSteps["^>>"] }, 
                                                { "45", midSteps[">"] },    { "46", midSteps[">>"] },  
                { "41", midSteps["v"] },        { "42", midSteps["v>"] },   { "43", midSteps["v>>"] },
                                                { "40", midSteps["Lvv>"] }, { "4A", midSteps["Lvv>>"] },

                { "57", midSteps["^<"] },       { "58", midSteps["^"] },    { "59", midSteps["^>"] }, 
                { "54", midSteps["<"] },                                    { "56", midSteps[">"] },  
                { "51", midSteps["v<"] },       { "52", midSteps["v"] },    { "53", midSteps["v>"] },
                                                { "50", midSteps["vv"] },   { "5A", midSteps["vv>"] },

                { "67", midSteps["^<<"] },      { "68", midSteps["^<"] },   { "69", midSteps["^"] }, 
                { "64", midSteps["<<"] },       { "65", midSteps["<"] },  
                { "61", midSteps["v<<"] },      { "62", midSteps["v<"] },   { "63", midSteps["v"] },
                                                { "60", midSteps["vv<"] },  { "6A", midSteps["vv"] },

                                                { "78", midSteps[">"] },    { "79", midSteps[">>"] }, 
                { "74", midSteps["v"] },        { "75", midSteps["v>"] },   { "76", midSteps["v>>"] },  
                { "71", midSteps["vv"] },       { "72", midSteps["vv>"] },  { "73", midSteps["vv>>"] },
                                                { "70", midSteps["Lvvv>"] },{ "7A", midSteps["Lvvv>>"] },

                { "87", midSteps["<"] },                                    { "89", midSteps[">"] }, 
                { "84", midSteps["v<"] },       { "85", midSteps["v"] },    { "86", midSteps["v>"] },  
                { "81", midSteps["vv<"] },      { "82", midSteps["vv"] },   { "83", midSteps["vv>"] },
                                                { "80", midSteps["vvv"] },  { "8A", midSteps["vvv>"] },

                { "97", midSteps["<<"] },       { "98", midSteps["<"] },
                { "94", midSteps["v<<"] },      { "95", midSteps["v<"] },   { "96", midSteps["v"] }, 
                { "91", midSteps["vv<<"] },     { "92", midSteps["vv<"] },  { "93", midSteps["vv"] },
                                                { "90", midSteps["vvv<"] }, { "9A", midSteps["vvv"] },
            };
        }
    }
}
