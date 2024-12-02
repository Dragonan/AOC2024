using System;

namespace AOC2024
{
    public static class Day2
    {
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var reports = GetReports();
            var safeReports = 0;

            foreach (var report in reports)
            {
                if (CheckReport(report, false))
                    safeReports++;
            }

            Console.WriteLine(safeReports);
        }

        private static void SolvePart2()
        {
            var reports = GetReports();
            var safeReports = 0;

            foreach (var report in reports)
            {
                if (CheckReport(report, true))
                    safeReports++;
            }

            Console.WriteLine(safeReports);
        }

        private static int[][] GetReports()
        {
            var lines = PuzzleInput.Input;
            var reports = new int[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                reports[i] = lines[i].Split(' ').Select(int.Parse).ToArray();
            }

            return reports;
        }

        private static bool CheckReport(int[] report, bool checkWithRemoval)
        {
            if (report[0] == report[1])
            {
                if (checkWithRemoval)
                    return CheckReport(report.Skip(1).ToArray(), false);
                else
                    return false;
            }

            var asc = report[0] < report[1];
            var isSafe = true;

            for (int i = 0; i < report.Length - 1; i++)
            {
                if (AreLevelsSafe(asc, report[i], report[i+1]))
                {
                    continue;
                }
                else if (checkWithRemoval && 
                        (CheckReport(report.RemoveAt(i), false) || 
                        CheckReport(report.RemoveAt(i+1), false) ||
                        CheckReport(report.Skip(1).ToArray(), false)))
                {
                    break;
                }

                isSafe = false;
                break;
            }

            return isSafe;
        }

        private static bool AreLevelsSafe(bool asc, int a, int b)
        {
            return (asc && a < b && b - a < 4) || (!asc && a > b && a - b < 4);
        }
    }
}
