namespace LogHub.Core;

public interface ILogFilter
{
    bool IsValid(LogEntry entry);
}
