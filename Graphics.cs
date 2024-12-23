using System.Drawing;

namespace AOC2024
{
    public static class Graphics
    {
        public static void Draw(IEnumerable<string> lines, bool clear = true)
        {
            Console.CursorVisible = false;
            if (clear)
                Console.Clear();
            Console.SetCursorPosition(0, 0);
            foreach (var line in lines)
                Console.WriteLine(line);
        }

        public static void DrawLine(string line, int[]? toColor = null, ConsoleColor color = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black)
        {
            toColor ??= new int[0];
            Console.CursorVisible = false;
            Console.SetCursorPosition(0,Console.GetCursorPosition().Top-1);
            for (int i = 0; i < line.Length; i++)
            {
                if (toColor.Contains(i))
                {
                    Console.ForegroundColor = color;
                    Console.BackgroundColor = background;
                }

                Console.Write(line[i]);
                
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            
            Console.WriteLine(new string(' ', Console.WindowWidth - line.Length));
        }

        public static void DrawWithColor(string[] lines, List<Point> toColor, ConsoleColor color, ConsoleColor background = ConsoleColor.Black, bool clear = true)
        {
            Console.CursorVisible = false;
            if (clear)
                Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (toColor.Contains(new (j,i)))
                    {
                        Console.ForegroundColor = color;
                        Console.BackgroundColor = background;
                    }
                    Console.Write(lines[i][j]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }

        public static void DrawWithColors(string[] lines, List<List<Point>> spaces, ConsoleColor[] colors, ConsoleColor[] backgrounds)
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    for (int c = 0; c < colors.Length; c++)
                    {
                        if (spaces[c].Contains(new (j,i)))
                        {
                            Console.ForegroundColor = colors[c];
                            Console.BackgroundColor = backgrounds[c];
                        }
                    }
                    Console.Write(lines[i][j]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }
    }
}
