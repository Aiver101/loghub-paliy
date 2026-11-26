namespace LogHub.Core;

public abstract class LogImporterBase
{
    protected readonly ILogParser Parser;
    protected readonly ILogFilter Filter;
    protected readonly ILogFormatter Formatter;
    protected readonly IEnumerable<ILogWriter> Writers;

    protected LogImporterBase(
        ILogParser parser,
        ILogFilter filter,
        ILogFormatter formatter,
        IEnumerable<ILogWriter> writers)
    {
        Parser = parser;
        Filter = filter;
        Formatter = formatter;
        Writers = writers;
    }

    // Шаблонный метод — задает скелет алгоритма
    public void Import(string sourcePath)
    {
        var lines = ReadLines(sourcePath); // Шаг 1 (реализуется в наследниках)

        foreach (var line in lines)
        {
            var entry = ParseLine(line); // Шаг 2 (реализуется в наследниках)
            if (entry == null) continue;

            if (!Filter.IsValid(entry)) continue; // Шаг 3

            var formatted = Formatter.Format(entry); // Шаг 4

            foreach (var writer in Writers)
            {
                writer.Write(formatted); // Шаг 5
            }
        }
    }

    // Абстрактные методы для переопределения (Стратегия)
    protected abstract IEnumerable<string> ReadLines(string sourcePath);
    protected abstract LogEntry? ParseLine(string line);
}