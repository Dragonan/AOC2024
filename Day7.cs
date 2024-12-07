using System;

namespace AOC2024
{
    public class TestEquation
    {
        public ulong Value { get; set; }
        public ulong[] Numbers { get; set; }

        public TestEquation(ulong value, ulong[] numbers)
        {
            Value = value;
            Numbers = numbers;
        }
    }

    public class Day7
    {
        public static void Solve()
        {
            SolvePart2();
        }

        private static void SolvePart1()
        {
            var tests = GetTests();
            var sum = 0UL;
            foreach (var test in tests)
                if (CheckEquation(test))
                    sum += test.Value;
            
            Console.WriteLine(sum);
        }

        private static void SolvePart2()
        {
            var tests = GetTests();
            UInt128 sum = 0;
            foreach (var test in tests)
                if (CheckEquation(test, true))
                    sum += test.Value;
            
            Console.WriteLine(sum);
        }

        private static List<TestEquation> GetTests()
        {
            var result = new List<TestEquation>();
            var lines = PuzzleInput.Input;
            foreach (var line in lines)
            {
                var equation = line.Split(": ");
                result.Add(new (ulong.Parse(equation[0]), equation[1].Split(' ').Select(ulong.Parse).ToArray()));
            }
            return result;
        }

        private static bool CheckEquation(TestEquation test, bool checkConcat = false)
        {
            var sum = test.Numbers[0] + test.Numbers[1];
            var product = test.Numbers[0] * test.Numbers[1];
            var concat = checkConcat ? ulong.Parse("" + test.Numbers[0] + test.Numbers[1]) : 0;

            if (test.Numbers.Length > 2)
            {
                var leftovers = test.Numbers.Skip(2);
                return (sum <= test.Value && CheckEquation(new (test.Value, leftovers.Prepend(sum).ToArray()), checkConcat)) || 
                       (product <= test.Value && CheckEquation(new (test.Value, leftovers.Prepend(product).ToArray()), checkConcat)) ||
                       (checkConcat && concat <= test.Value && CheckEquation(new (test.Value, leftovers.Prepend(concat).ToArray()), checkConcat));
            }

            return test.Value == sum || test.Value == product || (checkConcat && test.Value == concat);
        }
    }
}
