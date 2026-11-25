namespace LogHub.Core;

public class PlainTextLogParser : ILogParser
{
    public LogEntry? Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;

        // Ожидаем формат: [дата] УРОВЕНЬ сообщение
        var parts = line.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length < 3) return null; // Плохая строка

        return new LogEntry
        {
            Date = parts[0],
            Level = parts[1],
            Message = parts[2]
        };
    }
}
