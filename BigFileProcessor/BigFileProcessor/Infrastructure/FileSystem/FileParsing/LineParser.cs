using System.Text.RegularExpressions;

namespace BigFileProcessor.Infrastructure.FileSystem.FileParsing;

public static class LineParser
{
    private const string HeaderRowName = "HDR";
    private const string LineRowName = "LINE";

    private static readonly Regex SplitRegex = new(@"\s+", RegexOptions.Compiled);

    public static ParsedLine? Parse(string line)
    {
        var parts = SplitRegex.Split(line.Trim());

        if (parts[0] == HeaderRowName && parts.Length == 3) return new HeaderLine(parts[1], parts[2]);

        if (parts[0] == LineRowName && parts.Length == 4)
            return new ContentLine(parts[1], parts[2], int.Parse(parts[3]));

        return null;
    }
}