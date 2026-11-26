using System.Text.Json;

namespace LogHub.Core;

public class JsonLogParser : ILogParser
{
    public LogEntry? Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;

        try
        {
            var doc = JsonDocument.Parse(line);
            var root = doc.RootElement;

            if (!root.TryGetProperty("date", out var dateProp) ||
                !root.TryGetProperty("level", out var levelProp) ||
                !root.TryGetProperty("message", out var messageProp))
            {
                return null;
            }

            return new LogEntry
            {
                Date = dateProp.GetString() ?? "",
                Level = levelProp.GetString() ?? "",
                Message = messageProp.GetString() ?? ""
            };
        }
        catch (JsonException)
        {
            return null;
        }
    }
}