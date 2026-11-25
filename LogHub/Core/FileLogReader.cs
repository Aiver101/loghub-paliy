namespace LogHub.Core;

public class FileLogReader : ILogReader
{
    public IEnumerable<string> ReadLines(string sourcePath)
    {
        if (!File.Exists(sourcePath))
            return Array.Empty<string>();

        return File.ReadLines(sourcePath);
    }
}