namespace AOC2024
{
    public static class Graphics
    {
        public static void Draw(IEnumerable<string> lines, bool clear = true)
        {
            if (clear)
                Console.Clear();
            Console.SetCursorPosition(0, 0);
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }
}
