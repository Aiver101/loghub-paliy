namespace LogHub.Core;

public class JsonLogImporter : LogImporterBase
{
    public JsonLogImporter(
        ILogParser parser,
        ILogFilter filter,
        ILogFormatter formatter,
        IEnumerable<ILogWriter> writers)
        : base(parser, filter, formatter, writers)
    {
    }

    protected override IEnumerable<string> ReadLines(string sourcePath)
    {
        if (!File.Exists(sourcePath))
            return Array.Empty<string>();

        return File.ReadLines(sourcePath);
    }

    protected override LogEntry? ParseLine(string line)
    {
        return Parser.Parse(line);
    }
}