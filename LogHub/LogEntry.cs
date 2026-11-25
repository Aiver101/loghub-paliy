namespace LogHub;

public sealed class LogEntry
{
    public string Date { get; init; } = "";
    public string Level { get; init; } = "";
    public string Message { get; init; } = "";
}