namespace LogHub.Core;

public class PlainTextLogFormatter : ILogFormatter
{
    public string Format(LogEntry entry)
    {
        return $"{entry.Date} | {entry.Level} | {entry.Message}";
    }
}
