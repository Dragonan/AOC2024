using System;

namespace AOC2024
{
    public class Day17
    {
        private static ulong A;
        private static ulong B;
        private static ulong C;
        private static int Pointer = 0;
        private static byte[] Program;
        private static List<byte> Output;

        public static void Solve()
        {
            Init();
            SolvePart2();
        }

        private static void SolvePart1()
        {
            while(Pointer < Program.Length)
                Execute(Program[Pointer++],Program[Pointer++]);
            Console.WriteLine(string.Join(',', Output));
        }

        private static void SolvePart2()
        {
            Console.WriteLine(GetRightInput(0, Program.Length-1));
        }

        private static void Init()
        {
            var input = PuzzleInput.Input;
            A = ulong.Parse(input[0].Substring(12));
            B = ulong.Parse(input[1].Substring(12));
            C = ulong.Parse(input[2].Substring(12));
            Program = input[4].Substring(9).Split(',').Select(byte.Parse).ToArray();
            Pointer = 0;
            Output = new List<byte>();
        }

        private static void Reset(ulong value)
        {
            A = value;
            B = 0;
            C = 0;
            Pointer = 0;
            Output.Clear();
        }

        private static ulong GetRightInput(ulong input, int index)
        {
            for (byte i = 0; i < 8; i++)
            {
                var testA = input*8 + i;
                Reset(testA);
                while(Pointer < Program.Length)
                    Execute(Program[Pointer++],Program[Pointer++], false);

                if (Output.Last() != Program[index])
                    continue;

                if (index == 0)
                    return testA;

                var result = GetRightInput(testA, index-1);
                if (result != 0)
                    return result;
            }

            return 0;
        }

        private static ulong GetCombo(byte value)
        {
            if (value < 4)
                return value;

            switch (value)
            {
                case 4: return A;
                case 5: return B;
                case 6: return C;
                case 7: throw new Exception("7 is not allowed!");
                default: throw new Exception("Unkown combo value");
            }
        }

        private static void Execute(byte opcode, byte value, bool loop = true)
        {
            switch (opcode)
            {
                case 0: Adv(value); break;
                case 1: Bxl(value); break;
                case 2: Bst(value); break;
                case 3: if (loop) Jnz(value); break;
                case 4: Bxc(); break;
                case 5: Out(value); break;
                case 6: Bdv(value); break;
                case 7: Cdv(value); break;
                default: throw new Exception("Unkown operand");
            }
        }

        /*0*/ private static void Adv(byte value) => A = (ulong)Math.Truncate(A/Math.Pow(2,GetCombo(value)));
        /*1*/ private static void Bxl(byte value) => B ^= value;
        /*2*/ private static void Bst(byte value) => B = GetCombo(value) % 8;
        /*3*/ private static void Jnz(byte value) { if (A != 0) Pointer = value; }
        /*4*/ private static void Bxc() => B ^= C;
        /*5*/ private static void Out(byte value) => Output.Add((byte)(GetCombo(value) % 8));
        /*6*/ private static void Bdv(byte value) => B = (ulong)Math.Truncate(A/Math.Pow(2,GetCombo(value)));
        /*7*/ private static void Cdv(byte value) => C = (ulong)Math.Truncate(A/Math.Pow(2,GetCombo(value)));
    }
}
