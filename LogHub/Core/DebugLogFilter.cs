namespace LogHub.Core;

public class DebugLogFilter : ILogFilter
{
    public bool IsValid(LogEntry entry)
    {
        // Отбрасываем DEBUG
        return !string.Equals(entry.Level, "DEBUG", StringComparison.OrdinalIgnoreCase);
    }
}