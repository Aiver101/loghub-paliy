namespace LogHub.Core;

public interface ILogParser
{
    // Возвращает null, если строку не удалось разобрать
    LogEntry? Parse(string line);
}
