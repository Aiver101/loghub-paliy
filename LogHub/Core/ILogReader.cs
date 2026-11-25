namespace LogHub.Core;

public interface ILogReader
{
    IEnumerable<string> ReadLines(string sourcePath);
}
