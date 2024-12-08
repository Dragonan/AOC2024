using System.Drawing;

namespace AOC2024;

public static class CustomExtensions
{
    public static string[] GetLines(this string multiLineText)
    {
        return multiLineText.Split('\n');
    }

    public static string ReplaceAt(this string text, int index, char replacement)
    {
        var result = text.Substring(0, index) + replacement + text.Substring(index + 1);
        return result;
    }

    public static T[] RemoveAt<T>(this T[] array, int index)
    {
        return array.Take(index).Concat(array.Skip(index + 1)).ToArray();
    }

    public static bool IsOutOfBounds(this Point point, int lengthX, int lengthY = 0)
    {
        if (lengthY == 0)
            lengthY = lengthX;

        return point.X >= lengthX || 0 > point.X ||
            point.Y >= lengthY || 0 > point.Y;
    }
}
