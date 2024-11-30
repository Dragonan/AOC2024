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

}
