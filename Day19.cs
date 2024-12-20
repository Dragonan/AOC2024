namespace AOC2024
{
    public class Day19
    {
        private static string[] Towels;
        private static string[] Patterns;
        private static Dictionary<int, string[]> TowelGroups;

        public static void Solve()
        {
            GetTowelsAndPatterns();
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var possiblePatterns = 0;
            foreach (var pattern in Patterns)
                if (CheckPattern(pattern))
                    possiblePatterns++;

            Console.WriteLine(possiblePatterns);
        }

        private static void SolvePart2()
        {
            var arrangements = 0L;
            foreach (var pattern in Patterns)
                arrangements += GetAllCombinations(pattern);

            Console.WriteLine(arrangements);
        }

        private static void GetTowelsAndPatterns()
        {
            var lines = PuzzleInput.Input;
            Towels = lines[0].Split(", ").OrderByDescending(t => t.Length).ToArray();
            Patterns = lines.Skip(2).ToArray();
            TowelGroups = Towels.GroupBy(t => t.Length).OrderBy(t => t.Key).ToDictionary(t => t.Key, t => t.ToArray());
        }

        private static bool CheckPattern(string pattern)
        {
            var frontMatches = new List<string> { "" };
            var backMatches = new List<string> { "" };

            while (frontMatches.Any() && backMatches.Any())
            {
                if (frontMatches.First().Length + backMatches.First().Length >= pattern.Length) {
                    if (frontMatches.Any(front => backMatches.Any(back => front.Length + back.Length == pattern.Length)))
                        return true;
                    
                    while (frontMatches.First().Length + backMatches.Last().Length > pattern.Length)
                    {
                        frontMatches.RemoveAt(0);
                        if (!frontMatches.Any())
                            return false;
                    }
                    while (frontMatches.Last().Length + backMatches.First().Length > pattern.Length)
                    {
                        backMatches.RemoveAt(0);
                        if (!backMatches.Any())
                            return false;
                    }
                }

                var newFrontMatches = new List<string>();
                foreach (var match in frontMatches)
                {
                    var toMatch = pattern.Substring(match.Length);
                    foreach (var towel in Towels)
                        if (toMatch.StartsWith(towel))
                            newFrontMatches.Add(match + towel);
                }
                var newBackMatches = new List<string>();
                foreach (var match in backMatches)
                {
                    var toMatch = pattern.Substring(0, pattern.Length - match.Length);
                    foreach (var towel in Towels)
                        if (toMatch.EndsWith(towel))
                            newBackMatches.Add(towel + match);
                }
                frontMatches = newFrontMatches.OrderByDescending(m => m.Length).ToList();
                backMatches = newBackMatches.OrderByDescending(m => m.Length).ToList();
            }
            
            return false;
        }

        private static long GetAllCombinations(string pattern)
        {
            var valid = new long[pattern.Length+1];
            for (int i = 1; i <= pattern.Length; i++)
            {
                foreach (var stripesCount in TowelGroups.Keys)
                {
                    var check = i - stripesCount;
                    var known = check < 0 ? 0 : check;
                    if (known + stripesCount > i)
                        break;

                    var multiplier = known > 0 ? valid[known] : 1L;
                    if (multiplier == 0)
                        continue;
                    
                    var toMatch = pattern.Substring(known, stripesCount);
                    foreach (var towel in TowelGroups[stripesCount])
                    {
                        if (toMatch == towel)
                        {
                            valid[known+stripesCount] += multiplier;
                            break;
                        }
                    }
                }
            }

            return valid.Last();
        }
    }
}
