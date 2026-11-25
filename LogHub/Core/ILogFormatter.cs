namespace LogHub.Core;

public interface ILogFormatter
{
    string Format(LogEntry entry);
}
